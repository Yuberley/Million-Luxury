namespace MillionLuxury.Application.Users.LogIn;

using MillionLuxury.Application.Common.Abstractions.CQRS;

public record LogInUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;