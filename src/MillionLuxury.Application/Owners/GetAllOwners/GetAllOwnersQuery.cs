namespace MillionLuxury.Application.Owners.GetAllOwners;

#region Usings
using MillionLuxury.Application.Common.Abstractions.CQRS;
using MillionLuxury.Application.Owners.Dtos;
#endregion

public record GetAllOwnersQuery() : IQuery<IEnumerable<OwnerResponse>>;
