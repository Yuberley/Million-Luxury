namespace MillionLuxury.Application.Owners.UpdateOwnerPhoto;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Application.Common.Extensions;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.File;
using MillionLuxury.Domain.Owners;
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

        var imageBytes = Convert.FromBase64String(request.Request.Base64Content);
        if (imageBytes.Length > MaxImageSizeBytes)
        {
            return Result.Failure(OwnerErrors.PhotoTooLarge);
        }

        if (!imageBytes.IsValidImageFormat())
        {
            return Result.Failure(OwnerErrors.InvalidPhotoFormat);
        }

        if (!string.IsNullOrEmpty(owner.PhotoPath))
        {
            var oldFileEntity = File.CreateForDelete(owner.PhotoPath, OwnerPhotosBucket);
            await storageService.DeleteFile(oldFileEntity);
        }

        var imageId = Guid.NewGuid();
        var fileExtension = Path.GetExtension(request.Request.FileName);
        var uniqueFileName = $"{imageId}{fileExtension}";
        var photoPath = $"owners/{owner.Id}";

        var fileEntity = File.CreateForSave(
            uniqueFileName,
            request.Request.Base64Content,
            photoPath,
            OwnerPhotosBucket,
            fileExtension.GetContentType()
        );

        var fullPathPhoto = await storageService.SaveFile(fileEntity);

        owner.UpdatePhoto(fullPathPhoto, dateTimeProvider.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
