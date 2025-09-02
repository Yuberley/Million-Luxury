namespace MillionLuxury.Application.Properties.Dtos;

#region Usings
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public record UpdatePropertyRequest(
    string Name,
    Address Address,
    int Year,
    int Status,
    PropertyDetails Details
);
