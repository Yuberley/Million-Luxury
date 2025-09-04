namespace MillionLuxury.Application.Properties.Dtos;

public record UpdatePropertyRequest(
    Guid OwnerId,
    string Name,
    Address Address,
    decimal Price,
    string Currency,
    int Year,
    Dtos.PropertyStatus Status,
    PropertyDetails Details
);
