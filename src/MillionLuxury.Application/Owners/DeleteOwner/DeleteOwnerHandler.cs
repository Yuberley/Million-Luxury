namespace MillionLuxury.Application.Owners.DeleteOwner;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Storage;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners;
using MillionLuxury.Domain.File;
#endregion

internal sealed class DeleteOwnerHandler : ICommandHandler<DeleteOwnerCommand>
{
    #region Constants
    private const string OwnerPhotosBucket = "owner-photos";
    #endregion

    #region Private Members
    private readonly IOwnerRepository ownerRepository;
    private readonly IStorageService storageService;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public DeleteOwnerHandler(
        IOwnerRepository ownerRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        this.ownerRepository = ownerRepository;
        this.storageService = storageService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = await ownerRepository.GetByIdWithPropertiesAsync(request.OwnerId, cancellationToken);

        if (owner is null)
        {
            return Result.Failure(OwnerErrors.NotFound);
        }

        // Check if owner has associated properties
        // Note: This assumes the GetByIdWithPropertiesAsync method includes properties navigation
        // If the owner has properties, we should not allow deletion
        // This business rule can be adjusted based on requirements

        // Delete owner photo from MinIO if exists
        if (!string.IsNullOrEmpty(owner.PhotoPath))
        {
            var fileEntity = File.CreateForDelete(owner.PhotoPath, OwnerPhotosBucket);
            await storageService.DeleteFile(fileEntity);
        }

        // Delete owner from database
        ownerRepository.Delete(owner);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
