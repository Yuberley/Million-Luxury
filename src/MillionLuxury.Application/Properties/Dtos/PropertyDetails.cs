namespace MillionLuxury.Application.Properties.Dtos;

public record PropertyDetails(
    PropertyType PropertyType,
    int Bedrooms,
    int Bathrooms,
    decimal AreaInSquareMeters,
    string Description
);
