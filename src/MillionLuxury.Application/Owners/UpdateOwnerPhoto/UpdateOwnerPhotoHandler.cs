namespace MillionLuxury.Application.Owners.UpdateOwnerPhoto;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners;
using MillionLuxury.Domain.File;
#endregion

internal sealed class UpdateOwnerPhotoHandler : ICommandHandler<UpdateOwnerPhotoCommand>
{
    #region Constants
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB
    private const string OwnerPhotosBucket = "owner-photos";
    #endregion

    #region Private Members
    private readonly IOwnerRepository ownerRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IStorageService storageService;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public UpdateOwnerPhotoHandler(
        IOwnerRepository ownerRepository,
        IDateTimeProvider dateTimeProvider,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        this.ownerRepository = ownerRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.storageService = storageService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOwnerPhotoCommand request, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetByIdAsync(request.OwnerId, cancellationToken);

        if (owner is null)
        {
            return Result.Failure(OwnerErrors.NotFound);
        }

        // Validate image size
        var imageBytes = Convert.FromBase64String(request.Request.Base64Content);
        if (imageBytes.Length > MaxImageSizeBytes)
        {
            return Result.Failure(OwnerErrors.PhotoTooLarge);
        }

        // Validate image format
        if (!IsValidImageFormat(imageBytes))
        {
            return Result.Failure(OwnerErrors.InvalidPhotoFormat);
        }

        // Delete old photo if exists
        if (!string.IsNullOrEmpty(owner.PhotoPath))
        {
            var oldFileEntity = File.CreateForDelete(owner.PhotoPath, OwnerPhotosBucket);
            await storageService.DeleteFile(oldFileEntity);
        }

        // Generate unique file name for MinIO storage
        var fileExtension = Path.GetExtension(request.Request.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var photoPath = $"owners/{owner.Id}";

        // Create File entity for MinIO storage
        var fileEntity = File.CreateForSave(
            uniqueFileName,
            request.Request.Base64Content,
            photoPath,
            OwnerPhotosBucket,
            GetContentType(fileExtension)
        );

        // Save file to MinIO
        await storageService.SaveFile(fileEntity);

        // Generate the full URL for the stored photo
        var photoUrl = $"{OwnerPhotosBucket}/{photoPath}/{uniqueFileName}";

        // Update owner photo
        owner.UpdatePhoto(photoUrl, dateTimeProvider.UtcNow);

        ownerRepository.Update(owner);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #region Private Methods
    private static bool IsValidImageFormat(byte[] imageBytes)
    {
        // Check for common image file signatures
        if (imageBytes.Length < 4) return false;

        // JPEG
        if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8) return true;

        // PNG
        if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47) return true;

        // WebP
        if (imageBytes.Length >= 12 &&
            imageBytes[0] == 0x52 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x46 &&
            imageBytes[8] == 0x57 && imageBytes[9] == 0x45 && imageBytes[10] == 0x42 && imageBytes[11] == 0x50) return true;

        return false;
    }

    private static string GetContentType(string fileExtension)
    {
        return fileExtension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
    #endregion
}
