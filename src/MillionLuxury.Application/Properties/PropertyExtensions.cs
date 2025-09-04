namespace MillionLuxury.Application.Properties;

#region Usings
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.SharedValueObjects;
using DomainPropertyStatus = Domain.Properties.ValueObjects.PropertyStatus;
using DomainPropertyType = Domain.Properties.ValueObjects.PropertyType;
using DtoPropertyStatus = Dtos.PropertyStatus;
using DtoPropertyType = Dtos.PropertyType;
#endregion

internal static class PropertyExtensions
{
    internal static Dtos.PropertyResponse ToDto(this Property property)
    {
        return new Dtos.PropertyResponse(
            property.Id,
            property.Name,
            Address: $"{property.Address.Street}, {property.Address.City}, {property.Address.State}, {property.Address.Country}",
            property.Price.Amount,
            property.Price.Currency.Code,
            property.InternalCode,
            property.Year,
            property.OwnerId,
            property.Status.ToDto(),
            property.Details.ToDto(),
            property.CreatedAt,
            property.UpdatedAt,
            property.Images.Select(img => img.ToDto())
        );
    }

    internal static Dtos.PropertyImageResponse ToDto(this PropertyImage image)
    {
        return new Dtos.PropertyImageResponse(
            image.Id,
            image.FileName,
            image.FilePath,
            image.IsEnabled,
            image.CreatedAt
        );
    }

    internal static Property ToDomain(this Dtos.CreatePropertyRequest request, DateTime createdAt)
    {
        return Property.Create(
            request.Name,
            request.Address.ToDomain(),
            new Money(request.Price, Currency.FromCode(request.Currency)),
            request.InternalCode,
            request.Year,
            request.OwnerId,
            request.Details.ToDomain(),
            createdAt
        );
    }

    internal static Dtos.SearchPropertiesResponse ToSearchResponse(
        this (IEnumerable<Property> Properties, int TotalCount) searchResult,
        int page,
        int pageSize)
    {
        var totalPages = (int)Math.Ceiling((double)searchResult.TotalCount / pageSize);

        return new Dtos.SearchPropertiesResponse(
            searchResult.Properties.Select(p => p.ToDto()),
            searchResult.TotalCount,
            page,
            pageSize,
            totalPages
        );
    }

    internal static Dtos.Address ToDto(this Address address)
    {
        return new Dtos.Address(
            address.Street,
            address.City,
            address.State,
            address.ZipCode,
            address.Country
        );
    }

    internal static Address ToDomain(this Dtos.Address address)
    {
        return new Address(
            address.Country,
            address.State,
            address.City,
            address.ZipCode,
            address.Street
        );
    }

    internal static Dtos.PropertyDetails ToDto(this Domain.Properties.ValueObjects.PropertyDetails details)
    {
        return new Dtos.PropertyDetails(
            details.PropertyType.ToDto(),
            details.Bedrooms,
            details.Bathrooms,
            details.AreaInSquareMeters,
            details.Description
        );
    }

    internal static Domain.Properties.ValueObjects.PropertyDetails ToDomain(this Dtos.PropertyDetails details)
    {
        return new Domain.Properties.ValueObjects.PropertyDetails(
            details.PropertyType.ToDomain(),
            details.Bedrooms,
            details.Bathrooms,
            details.AreaInSquareMeters,
            details.Description
        );
    }

    internal static DtoPropertyType ToDto(this DomainPropertyType value)
        => (DtoPropertyType)(int)value;

    internal static DomainPropertyType ToDomain(this DtoPropertyType value)
    => Enum.IsDefined(typeof(DomainPropertyType), (int)value)
        ? (DomainPropertyType)(int)value
        : throw new ArgumentOutOfRangeException(nameof(value), $"Unknown PropertyType: {value}");

    internal static DtoPropertyStatus ToDto(this DomainPropertyStatus value)
            => (DtoPropertyStatus)(int)value;

    internal static DomainPropertyStatus ToDomain(this DtoPropertyStatus value)
        => Enum.IsDefined(typeof(DomainPropertyStatus), (int)value)
            ? (DomainPropertyStatus)(int)value
            : throw new ArgumentOutOfRangeException(nameof(value), $"Unknown PropertyStatus: {value}");

    internal static bool IsValidImageFormat(this byte[] imageBytes)
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
}
