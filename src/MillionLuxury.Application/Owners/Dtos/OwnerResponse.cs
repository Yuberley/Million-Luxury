namespace MillionLuxury.Application.Owners.Dtos;

public class OwnerResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public DateTime Birthday { get; init; }
    public string? Photo { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
