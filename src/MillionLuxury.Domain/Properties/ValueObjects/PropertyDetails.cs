namespace MillionLuxury.Domain.Properties.ValueObjects;

public sealed record PropertyDetails(
    PropertyType PropertyType,
    int Bedrooms,
    int Bathrooms,
    decimal AreaInSquareMeters,
    string Description
);