namespace MillionLuxury.Application.Properties.UpdateProperty;

#region Usings
using FluentValidation;
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public class UpdatePropertyInputValidation : AbstractValidator<UpdatePropertyCommand>
{
    #region Constants
    private const int MinYear = 1900;
    private const int MaxYear = 2100;
    private const int MinArea = 1;
    private const int MinBedrooms = 0;
    private const int MinBathrooms = 0;
    private const int MaxNameLength = 200;
    private const int MaxAddressLength = 50;
    private const int MaxDescriptionLength = 2000;
    #endregion

    public UpdatePropertyInputValidation()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MaximumLength(MaxNameLength);

        RuleFor(x => x.Request.Address.Country).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Request.Address.State).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Request.Address.City).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Request.Address.ZipCode).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Request.Address.Street).NotEmpty().MaximumLength(MaxAddressLength);

        RuleFor(x => x.Request.Year)
            .GreaterThanOrEqualTo(MinYear)
            .LessThanOrEqualTo(MaxYear);

        RuleFor(x => x.Request.Status)
            .Must(BeValidPropertyStatus)
            .WithMessage("Invalid property status");

        RuleFor(x => x.Request.Details.PropertyType.ToString())
            .Must(BeValidPropertyType)
            .WithMessage("Invalid property type");

        RuleFor(x => x.Request.Details.Bedrooms)
            .GreaterThanOrEqualTo(MinBedrooms);

        RuleFor(x => x.Request.Details.Bathrooms)
            .GreaterThanOrEqualTo(MinBathrooms);

        RuleFor(x => x.Request.Details.AreaInSquareMeters)
            .GreaterThanOrEqualTo(MinArea);

        RuleFor(x => x.Request.Details.Description)
            .NotEmpty()
            .MaximumLength(MaxDescriptionLength);
    }

    private static bool BeValidPropertyType(string propertyType)
    {
        return Enum.IsDefined(typeof(PropertyType), propertyType);
    }

    private static bool BeValidPropertyStatus(int status)
    {
        return Enum.IsDefined(typeof(PropertyStatus), status);
    }
}
