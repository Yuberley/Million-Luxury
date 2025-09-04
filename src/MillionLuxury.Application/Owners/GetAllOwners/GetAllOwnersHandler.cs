namespace MillionLuxury.Application.Owners.GetAllOwners;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Owners.Dtos;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners;
#endregion

internal sealed class GetAllOwnersHandler : IQueryHandler<GetAllOwnersQuery, IEnumerable<OwnerResponse>>
{
    #region Private Members
    private readonly IOwnerRepository ownerRepository;
    #endregion

    public GetAllOwnersHandler(IOwnerRepository ownerRepository)
    {
        this.ownerRepository = ownerRepository;
    }

    public async Task<Result<IEnumerable<OwnerResponse>>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
    {
        var owners = await ownerRepository.GetAllAsync(cancellationToken);

        var ownersResponse = owners.Select(owner => owner.ToDto());

        return Result.Success(ownersResponse);
    }
}
