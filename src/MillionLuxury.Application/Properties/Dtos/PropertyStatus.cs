namespace MillionLuxury.Application.Properties.Dtos;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyStatus
{
    [JsonPropertyName("Available")]
    Available = 1,

    [JsonPropertyName("Sold")]
    Sold = 2,

    [JsonPropertyName("Rented")]
    Rented = 3,

    [JsonPropertyName("Pending")]
    Pending = 4,

    [JsonPropertyName("Inactive")]
    Inactive = 5,

    [JsonPropertyName("UnderConstruction")]
    UnderConstruction = 6
}
