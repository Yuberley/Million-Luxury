namespace MillionLuxury.Application.Users.Register;

using FluentValidation;

public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(c => c.Email).EmailAddress();

        RuleFor(c => c.Password).NotEmpty().MinimumLength(5);

        RuleFor(c => c.Roles).NotEmpty();
    }
}