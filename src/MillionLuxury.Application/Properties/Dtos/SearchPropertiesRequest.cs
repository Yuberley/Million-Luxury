namespace MillionLuxury.Application.Properties.Dtos;

public record SearchPropertiesRequest(
    int Page = 1,
    int PageSize = 10,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int? PropertyType = null,
    int? Status = null,
    int? MinBedrooms = null,
    int? MaxBedrooms = null,
    int? MinBathrooms = null,
    int? MaxBathrooms = null,
    decimal? MinArea = null,
    decimal? MaxArea = null
);
