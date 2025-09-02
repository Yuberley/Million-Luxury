namespace MillionLuxury.Application.Properties.RemoveImage;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.File;
#endregion

internal sealed class RemoveImageHandler : ICommandHandler<RemoveImageCommand>
{
    #region Constants
    private const string PropertyImagesBucket = "property-images";
    #endregion

    #region Private Members
    private readonly IPropertyRepository propertyRepository;
    private readonly IStorageService storageService;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public RemoveImageHandler(
        IPropertyRepository propertyRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        this.propertyRepository = propertyRepository;
        this.storageService = storageService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdWithImagesAsync(request.PropertyId, cancellationToken);

        if (property is null)
        {
            return Result.Failure(PropertyErrors.NotFound);
        }

        var image = property.Images.FirstOrDefault(i => i.Id == request.ImageId);
        if (image is null)
        {
            return Result.Failure(PropertyErrors.ImageNotFound);
        }

        // Remove image from MinIO storage
        var fileEntity = File.CreateForDelete(image.FilePath, PropertyImagesBucket);
        await storageService.DeleteFile(fileEntity);

        // Remove image from property
        property.RemoveImage(request.ImageId);

        propertyRepository.Update(property);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
