namespace MillionLuxury.Application.Properties.Dtos;

public record Address(
    string Country,
    string State,
    string City,
    string ZipCode,
    string Street);