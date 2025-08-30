namespace MillionLuxury.Application.Users.Register;

using MillionLuxury.Application.Common.Abstractions.CQRS;

public record RegisterUserCommand(string Email, string Password, string[] Roles) : ICommand<Guid>;