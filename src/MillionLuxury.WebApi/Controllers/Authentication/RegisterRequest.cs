namespace MillionLuxury.Web.API.Controllers.Authentication;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string[] Roles
    );