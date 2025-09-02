namespace MillionLuxury.WebApi.Controllers.Authentication;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string[] Roles
    );