namespace MillionLuxury.Application.Properties.Dtos;

public record SearchPropertiesRequest(
    int Page = 1,
    int PageSize = 10,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    PropertyType? PropertyType = null
);
