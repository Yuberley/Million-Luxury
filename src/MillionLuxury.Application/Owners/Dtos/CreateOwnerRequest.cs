namespace MillionLuxury.Application.Owners.Dtos;

using MillionLuxury.Application.Properties.Dtos;

public class CreateOwnerRequest
{
    public string Name { get; init; }
    public Address Address { get; init; }
    public DateTime Birthday { get; init; }
    public string? Photo { get; init; }
}
