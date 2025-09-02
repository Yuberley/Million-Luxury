namespace MillionLuxury.Application.Properties.AddImage;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.File;
#endregion

internal sealed class AddImageHandler : ICommandHandler<AddImageCommand, Guid>
{
    #region Constants
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB
    private const string PropertyImagesBucket = "property-images";
    #endregion

    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IStorageService storageService;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public AddImageHandler(
        IPropertyRepository propertyRepository,
        IDateTimeProvider dateTimeProvider,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        this.propertyRepository = propertyRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.storageService = storageService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdWithImagesAsync(request.PropertyId, cancellationToken);

        if (property is null)
        {
            return Result.Failure<Guid>(PropertyErrors.NotFound);
        }

        // Validate image size
        var imageBytes = Convert.FromBase64String(request.Request.Base64Content);
        if (imageBytes.Length > MaxImageSizeBytes)
        {
            return Result.Failure<Guid>(PropertyErrors.ImageTooLarge);
        }

        // Validate image format
        if (!IsValidImageFormat(imageBytes))
        {
            return Result.Failure<Guid>(PropertyErrors.InvalidImageFormat);
        }

        // Generate unique file name for MinIO storage
        var fileExtension = Path.GetExtension(request.Request.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var imagePath = $"properties/{property.Id}";

        // Create File entity for MinIO storage
        var fileEntity = File.CreateForSave(
            uniqueFileName,
            request.Request.Base64Content,
            imagePath,
            PropertyImagesBucket,
            GetContentType(fileExtension)
        );

        // Save file to MinIO
        await storageService.SaveFile(fileEntity);

        // Generate the full URL for the stored image
        var imageUrl = $"{PropertyImagesBucket}/{imagePath}/{uniqueFileName}";

        // Create and add the image entity with MinIO URL
        var image = PropertyImage.Create(
            property.Id,
            request.Request.FileName,
            imageUrl,
            dateTimeProvider.UtcNow
        );

        property.AddImage(image);

        propertyRepository.Update(property);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(image.Id);
    }

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
}
