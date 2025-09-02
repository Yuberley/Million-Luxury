namespace MillionLuxury.Application.Properties.Dtos;

#region Usings
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public record CreatePropertyRequest(
    string Name,
    Address Address,
    decimal Price,
    string Currency,
    string InternalCode,
    int Year,
    PropertyDetails Details
);
