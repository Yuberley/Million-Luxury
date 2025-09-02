namespace MillionLuxury.Application.Properties.Dtos;

public record ChangePriceRequest(
    decimal Price,
    string Currency
);
