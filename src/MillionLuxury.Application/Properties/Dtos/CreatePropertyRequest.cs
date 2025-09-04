namespace MillionLuxury.Application.Properties.Dtos;

public record CreatePropertyRequest(
    Guid OwnerId,
    string Name,
    Address Address,
    decimal Price,
    string Currency,
    string InternalCode,
    int Year,
    PropertyDetails Details
);
