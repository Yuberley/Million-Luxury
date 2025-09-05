namespace MillionLuxury.Application.Properties.UpdateProperty;

#region Usings
using FluentValidation;
using MillionLuxury.Domain.Properties;
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

        RuleFor(x => x.Property.OwnerId)
            .NotEmpty()
            .GetType()
            .Equals(typeof(Guid));

        RuleFor(x => x.Property.Name)
            .NotEmpty()
            .MaximumLength(MaxNameLength);

        RuleFor(x => x.Property.Address.Country).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Property.Address.State).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Property.Address.City).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Property.Address.ZipCode).NotEmpty().MaximumLength(MaxAddressLength);
        RuleFor(x => x.Property.Address.Street).NotEmpty().MaximumLength(MaxAddressLength);

        RuleFor(x => x.Property.Year)
            .GreaterThanOrEqualTo(MinYear)
            .LessThanOrEqualTo(MaxYear)
            .WithMessage(PropertyErrors.InvalidYear.Name);

        RuleFor(x => x.Property.Status.ToString())
            .Must(BeValidPropertyStatus)
            .WithMessage("Invalid property status");

        RuleFor(x => x.Property.Details.PropertyType.ToString())
            .Must(BeValidPropertyType)
            .WithMessage("Invalid property type");

        RuleFor(x => x.Property.Details.Bedrooms)
            .GreaterThanOrEqualTo(MinBedrooms)
            .WithMessage(PropertyErrors.InvalidBedrooms.Name);

        RuleFor(x => x.Property.Details.Bathrooms)
            .GreaterThanOrEqualTo(MinBathrooms)
            .WithMessage(PropertyErrors.InvalidBathrooms.Name);

        RuleFor(x => x.Property.Details.AreaInSquareMeters)
            .GreaterThanOrEqualTo(MinArea)
            .WithMessage(PropertyErrors.InvalidArea.Name);

        RuleFor(x => x.Property.Details.Description)
            .NotEmpty()
            .MaximumLength(MaxDescriptionLength);
    }

    private static bool BeValidPropertyType(string propertyType)
    {
        return Enum.IsDefined(typeof(PropertyType), propertyType);
    }

    private static bool BeValidPropertyStatus(string status)
    {
        return Enum.IsDefined(typeof(PropertyStatus), status);
    }
}
