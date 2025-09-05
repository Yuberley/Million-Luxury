namespace MillionLuxury.Application.Properties.CreateProperty;

#region Usings
using FluentValidation;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public class CreatePropertyInputValidation : AbstractValidator<CreatePropertyCommand>
{
    #region Constants
    private const int MinPrice = 1;
    private const int MinYear = 1900;
    private const int MaxYear = 2100;
    private const int MinArea = 1;
    private const int MinBedrooms = 0;
    private const int MinBathrooms = 0;
    private const int MaxNameLength = 200;
    private const int MaxAddressLength = 50;
    private const int MaxInternalCodeLength = 50;
    private const int MaxDescriptionLength = 2000;
    #endregion

    public CreatePropertyInputValidation()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(MaxNameLength);

        RuleFor(c => c.Request.Address.Country).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(c => c.Request.Address.State).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(c => c.Request.Address.City).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(c => c.Request.Address.ZipCode).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(c => c.Request.Address.Street).NotEmpty().MaximumLength(MaxAddressLength);

        RuleFor(x => x.Request.Price)
            .GreaterThanOrEqualTo(MinPrice)
            .WithMessage(PropertyErrors.InvalidPrice.Name);

        RuleFor(x => x.Request.InternalCode)
            .NotEmpty()
            .MaximumLength(MaxInternalCodeLength);

        RuleFor(x => x.Request.Year)
            .ExclusiveBetween(MinYear, MaxYear)
            .WithMessage(PropertyErrors.InvalidYear.Name);

        RuleFor(x => x.Request.Details.PropertyType.ToString())
            .Must(BeValidPropertyType)
            .WithMessage("Invalid property type");

        RuleFor(x => x.Request.Details.Bedrooms)
            .GreaterThanOrEqualTo(MinBedrooms)
            .WithMessage(PropertyErrors.InvalidBedrooms.Name);

        RuleFor(x => x.Request.Details.Bathrooms)
            .GreaterThanOrEqualTo(MinBathrooms)
            .WithMessage(PropertyErrors.InvalidBathrooms.Name);

        RuleFor(x => x.Request.Details.AreaInSquareMeters)
            .GreaterThanOrEqualTo(MinArea)
            .WithMessage(PropertyErrors.InvalidArea.Name);

        RuleFor(x => x.Request.Details.Description)
            .NotEmpty()
            .MaximumLength(MaxDescriptionLength);
    }

    private static bool BeValidPropertyType(string propertyType)
    {
        return Enum.IsDefined(typeof(PropertyType), propertyType);
    }
}
