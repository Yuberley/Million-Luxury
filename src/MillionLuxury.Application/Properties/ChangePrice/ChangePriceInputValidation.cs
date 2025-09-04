namespace MillionLuxury.Application.Properties.ChangePrice;

#region Usings
using FluentValidation;
#endregion

public class ChangePriceInputValidation : AbstractValidator<ChangePriceCommand>
{
    #region Constants
    private const int MinPrice = 1;
    #endregion

    public ChangePriceInputValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.ChangePrice.Price)
            .GreaterThanOrEqualTo(MinPrice);
    }
}
