namespace MillionLuxury.Application.Properties.SearchProperties;

#region Usings
using FluentValidation;
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public class SearchPropertiesInputValidation : AbstractValidator<SearchPropertiesQuery>
{
    #region Constants
    private const int MinPage = 1;
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;
    private const int MinPrice = 0;
    private const int MinBedrooms = 0;
    private const int MinBathrooms = 0;
    private const int MinArea = 0;
    #endregion

    public SearchPropertiesInputValidation()
    {
        RuleFor(x => x.Request.Page)
            .GreaterThanOrEqualTo(MinPage);

        RuleFor(x => x.Request.PageSize)
            .GreaterThanOrEqualTo(MinPageSize)
            .LessThanOrEqualTo(MaxPageSize);

        RuleFor(x => x.Request.MinPrice)
            .GreaterThanOrEqualTo(MinPrice)
            .When(x => x.Request.MinPrice.HasValue);

        RuleFor(x => x.Request.MaxPrice)
            .GreaterThanOrEqualTo(MinPrice)
            .When(x => x.Request.MaxPrice.HasValue);

        RuleFor(x => x.Request)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("MinPrice must be less than or equal to MaxPrice");

        RuleFor(x => x.Request.PropertyType)
            .Must(BeValidPropertyType)
            .When(x => x.Request.PropertyType.HasValue)
            .WithMessage("Invalid property type");

        RuleFor(x => x.Request.Status)
            .Must(BeValidPropertyStatus)
            .When(x => x.Request.Status.HasValue)
            .WithMessage("Invalid property status");

        RuleFor(x => x.Request.MinBedrooms)
            .GreaterThanOrEqualTo(MinBedrooms)
            .When(x => x.Request.MinBedrooms.HasValue);

        RuleFor(x => x.Request.MaxBedrooms)
            .GreaterThanOrEqualTo(MinBedrooms)
            .When(x => x.Request.MaxBedrooms.HasValue);

        RuleFor(x => x.Request)
            .Must(x => !x.MinBedrooms.HasValue || !x.MaxBedrooms.HasValue || x.MinBedrooms <= x.MaxBedrooms)
            .WithMessage("MinBedrooms must be less than or equal to MaxBedrooms");

        RuleFor(x => x.Request.MinBathrooms)
            .GreaterThanOrEqualTo(MinBathrooms)
            .When(x => x.Request.MinBathrooms.HasValue);

        RuleFor(x => x.Request.MaxBathrooms)
            .GreaterThanOrEqualTo(MinBathrooms)
            .When(x => x.Request.MaxBathrooms.HasValue);

        RuleFor(x => x.Request)
            .Must(x => !x.MinBathrooms.HasValue || !x.MaxBathrooms.HasValue || x.MinBathrooms <= x.MaxBathrooms)
            .WithMessage("MinBathrooms must be less than or equal to MaxBathrooms");

        RuleFor(x => x.Request.MinArea)
            .GreaterThanOrEqualTo(MinArea)
            .When(x => x.Request.MinArea.HasValue);

        RuleFor(x => x.Request.MaxArea)
            .GreaterThanOrEqualTo(MinArea)
            .When(x => x.Request.MaxArea.HasValue);

        RuleFor(x => x.Request)
            .Must(x => !x.MinArea.HasValue || !x.MaxArea.HasValue || x.MinArea <= x.MaxArea)
            .WithMessage("MinArea must be less than or equal to MaxArea");
    }

    private static bool BeValidPropertyType(int? propertyType)
    {
        return propertyType.HasValue && Enum.IsDefined(typeof(PropertyType), propertyType.Value);
    }

    private static bool BeValidPropertyStatus(int? status)
    {
        return status.HasValue && Enum.IsDefined(typeof(PropertyStatus), status.Value);
    }
}
