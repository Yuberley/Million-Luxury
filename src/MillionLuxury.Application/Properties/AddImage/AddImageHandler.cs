namespace MillionLuxury.Application.Properties.AddImage;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Application.Common.Extensions;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.File;
using MillionLuxury.Domain.Properties;
#endregion

internal sealed class AddImageHandler : ICommandHandler<AddImageCommand, Guid>
{
    #region Constants
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB
    private const string PropertyImagesBucket = "property-images";
    #endregion

    #region Private Members
    private readonly IUnitOfWork unitOfWork;
    private readonly IStorageService storageService;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IPropertyRepository propertyRepository;
    private readonly IPropertyImagesRepository propertyImagesRepository;
    #endregion

    public AddImageHandler(
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        IDateTimeProvider dateTimeProvider,
        IPropertyRepository propertyRepository,
        IPropertyImagesRepository propertyImagesRepository)
    {
        this.unitOfWork = unitOfWork;
        this.storageService = storageService;
        this.dateTimeProvider = dateTimeProvider;
        this.propertyRepository = propertyRepository;
        this.propertyImagesRepository = propertyImagesRepository;
    }

    public async Task<Result<Guid>> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdWithImagesAsync(request.PropertyId, cancellationToken);

        if (property is null)
        {
            return Result.Failure<Guid>(PropertyErrors.NotFound);
        }

        var imageBytes = Convert.FromBase64String(request.AddImage.Base64Content);
        if (imageBytes.Length > MaxImageSizeBytes)
        {
            return Result.Failure<Guid>(PropertyErrors.ImageTooLarge);
        }

        if (!imageBytes.IsValidImageFormat())
        {
            return Result.Failure<Guid>(PropertyErrors.InvalidImageFormat);
        }

        var imageId = Guid.NewGuid();
        var fileExtension = Path.GetExtension(request.AddImage.FileName);
        var uniqueFileName = $"{imageId}{fileExtension}";
        var imagePath = $"properties/{property.Id}";

        var fileEntity = File.CreateForSave(
            uniqueFileName,
            request.AddImage.Base64Content,
            imagePath,
            PropertyImagesBucket,
            fileExtension.GetContentType()
        );

        var fullPathImage = await storageService.SaveFile(fileEntity);

        var image = PropertyImage.Create(
            imageId,
            property.Id,
            request.AddImage.FileName,
            fullPathImage,
            dateTimeProvider.UtcNow
        );

        property.AddImage(image);

        propertyImagesRepository.Add(image);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(image.Id);
    }
}
