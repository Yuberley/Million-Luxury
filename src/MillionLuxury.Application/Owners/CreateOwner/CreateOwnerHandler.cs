namespace MillionLuxury.Application.Owners.CreateOwner;

#region Usings
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners;
#endregion

internal sealed class CreateOwnerHandler : ICommandHandler<CreateOwnerCommand, Guid>
{
    #region Private Members
    private readonly IOwnerRepository ownerRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUnitOfWork unitOfWork;
    #endregion

    public CreateOwnerHandler(
        IOwnerRepository ownerRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        this.ownerRepository = ownerRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
    {
        // Check if owner with same name already exists
        if (await ownerRepository.ExistsByNameAsync(request.Request.Name, cancellationToken))
        {
            return Result.Failure<Guid>(OwnerErrors.NameAlreadyExists(request.Request.Name));
        }

        // Create new owner
        var owner = request.Request.ToDomain(dateTimeProvider.UtcNow);

        ownerRepository.Add(owner);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(owner.Id);
    }
}
