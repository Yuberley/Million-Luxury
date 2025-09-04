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
    }

    private static bool BeValidPropertyType(Dtos.PropertyType? propertyType)
    {
        return propertyType.HasValue && Enum.IsDefined(typeof(PropertyType), propertyType.Value);
    }
}
