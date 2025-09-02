namespace MillionLuxury.Application.Properties.Dtos;

#region Usings
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public record PropertyResponse(
    Guid Id,
    string Name,
    Address Address,
    decimal Price,
    string Currency,
    string InternalCode,
    int Year,
    Guid OwnerId,
    PropertyStatus Status,
    PropertyDetails Details,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<PropertyImageResponse> Images
);
