namespace MillionLuxury.Application.Owners.CreateOwner;

#region Usings
using FluentValidation;
#endregion

internal sealed class CreateOwnerInputValidation : AbstractValidator<CreateOwnerCommand>
{
    #region Constants
    private const int NameMaxLength = 100;
    private const int AddressMaxLength = 50;
    #endregion

    public CreateOwnerInputValidation()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .WithMessage("Owner name is required")
            .MaximumLength(NameMaxLength)
            .WithMessage($"Owner name cannot exceed {NameMaxLength} characters");

        RuleFor(x => x.Request.Address.Country).NotEmpty().MaximumLength(AddressMaxLength);
        RuleFor(x => x.Request.Address.State).NotEmpty().MaximumLength(AddressMaxLength);
        RuleFor(x => x.Request.Address.City).NotEmpty().MaximumLength(AddressMaxLength);
        RuleFor(x => x.Request.Address.ZipCode).NotEmpty().MaximumLength(AddressMaxLength);
        RuleFor(x => x.Request.Address.Street).NotEmpty().MaximumLength(AddressMaxLength);

        RuleFor(x => x.Request.Birthday)
            .NotEmpty()
            .WithMessage("Owner birthday is required")
            .LessThan(DateTime.Today)
            .WithMessage("Owner birthday must be in the past");
    }
}
