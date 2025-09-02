namespace MillionLuxury.Application.Properties.Dtos;

public record SearchPropertiesResponse(
    IEnumerable<PropertyResponse> Properties,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
