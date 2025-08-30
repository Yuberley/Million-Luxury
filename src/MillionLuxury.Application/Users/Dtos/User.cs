namespace MillionLuxury.Application.Users.Dtos;

public record User(
    Guid UserId,
    string Email,
    string[] Roles,
    bool IsEmailVerified,
    DateTime CreatedAt);