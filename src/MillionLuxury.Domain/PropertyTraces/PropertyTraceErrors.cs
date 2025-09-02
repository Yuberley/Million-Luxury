namespace MillionLuxury.Domain.PropertyTraces;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.PropertyTraces.Resources;
#endregion

public static class PropertyTraceErrors
{
    public static readonly Error NotFound = new(
        nameof(PropertyTraceResources.PropertyTraceNotFound),
        PropertyTraceResources.PropertyTraceNotFound);

    public static readonly Error CannotBeDeleted = new(
        nameof(PropertyTraceResources.PropertyTraceCannotBeDeleted),
        PropertyTraceResources.PropertyTraceCannotBeDeleted);

    public static readonly Error NameRequired = new(
        nameof(PropertyTraceResources.PropertyTraceNameRequired),
        PropertyTraceResources.PropertyTraceNameRequired);

    public static readonly Error NameMaxLength = new(
        nameof(PropertyTraceResources.PropertyTraceNameMaxLength),
        PropertyTraceResources.PropertyTraceNameMaxLength);

    public static readonly Error ValueRequired = new(
        nameof(PropertyTraceResources.PropertyTraceValueRequired),
        PropertyTraceResources.PropertyTraceValueRequired);

    public static readonly Error ValueMustBePositive = new(
        nameof(PropertyTraceResources.PropertyTraceValueMustBePositive),
        PropertyTraceResources.PropertyTraceValueMustBePositive);

    public static readonly Error TaxRequired = new(
        nameof(PropertyTraceResources.PropertyTraceTaxRequired),
        PropertyTraceResources.PropertyTraceTaxRequired);

    public static readonly Error TaxMustBePositive = new(
        nameof(PropertyTraceResources.PropertyTraceTaxMustBePositive),
        PropertyTraceResources.PropertyTraceTaxMustBePositive);

    public static readonly Error DateSaleRequired = new(
        nameof(PropertyTraceResources.PropertyTraceDateSaleRequired),
        PropertyTraceResources.PropertyTraceDateSaleRequired);

    public static readonly Error DateSaleInvalidDate = new(
        nameof(PropertyTraceResources.PropertyTraceDateSaleInvalidDate),
        PropertyTraceResources.PropertyTraceDateSaleInvalidDate);

    public static readonly Error PropertyIdRequired = new(
        nameof(PropertyTraceResources.PropertyTracePropertyIdRequired),
        PropertyTraceResources.PropertyTracePropertyIdRequired);
}
