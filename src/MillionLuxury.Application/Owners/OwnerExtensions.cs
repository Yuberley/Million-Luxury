namespace MillionLuxury.Application.Owners;

#region Usings
using MillionLuxury.Application.Owners.Dtos;
using MillionLuxury.Application.Properties;
using MillionLuxury.Domain.Owners;
#endregion

internal static class OwnerExtensions
{
    public static OwnerResponse ToDto(this Owner owner)
    {
        return new OwnerResponse
        {
            Id = owner.Id,
            Name = owner.Name,
            Address = $"{owner.Address.Street}, {owner.Address.City}, {owner.Address.State}, {owner.Address.Country}",
            Birthday = owner.Birthday,
            Photo = owner.PhotoPath,
            CreatedAt = owner.CreatedAt,
            UpdatedAt = owner.UpdatedAt
        };
    }

    public static Owner ToDomain(this CreateOwnerRequest request, DateTime createdAt)
    {
        return Owner.Create(
            request.Name,
            request.Address.ToDomain(),
            request.Photo,
            request.Birthday,
            createdAt
        );
    }
}
