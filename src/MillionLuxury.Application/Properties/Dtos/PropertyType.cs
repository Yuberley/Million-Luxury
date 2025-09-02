namespace MillionLuxury.Application.Properties.Dtos;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyType
{
    [JsonPropertyName("House")]
    House = 1,

    [JsonPropertyName("Apartment")]
    Apartment = 2,

    [JsonPropertyName("Condo")]
    Condo = 3,

    [JsonPropertyName("Townhouse")]
    Townhouse = 4,

    [JsonPropertyName("Villa")]
    Villa = 5,

    [JsonPropertyName("Penthouse")]
    Penthouse = 6,

    [JsonPropertyName("Studio")]
    Studio = 7,

    [JsonPropertyName("Duplex")]
    Duplex = 8,

    [JsonPropertyName("Commercial")]
    Commercial = 9,

    [JsonPropertyName("Land")]
    Land = 10
}
