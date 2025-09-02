namespace MillionLuxury.Domain.Properties;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties.Resources;
#endregion

public static class PropertyErrors
{
    public static readonly Error NotFound = new(
        nameof(PropertyResources.PropertyNotFound),
        PropertyResources.PropertyNotFound);

    public static readonly Error InvalidPrice = new(
        nameof(PropertyResources.PropertyInvalidPrice),
        PropertyResources.PropertyInvalidPrice);

    public static readonly Error InvalidYear = new(
        nameof(PropertyResources.PropertyInvalidYear),
        PropertyResources.PropertyInvalidYear);

    public static readonly Error InvalidArea = new(
        nameof(PropertyResources.PropertyInvalidArea),
        PropertyResources.PropertyInvalidArea);

    public static readonly Error InvalidBedrooms = new(
        nameof(PropertyResources.PropertyInvalidBedrooms),
        PropertyResources.PropertyInvalidBedrooms);

    public static readonly Error InvalidBathrooms = new(
        nameof(PropertyResources.PropertyInvalidBathrooms),
        PropertyResources.PropertyInvalidBathrooms);

    public static Error InternalCodeAlreadyExists(string internalCode) => new(
        nameof(PropertyResources.PropertyInternalCodeAlreadyExists),
        string.Format(PropertyResources.PropertyInternalCodeAlreadyExists, internalCode));

    public static readonly Error ImageNotFound = new(
        nameof(PropertyResources.PropertyImageNotFound),
        PropertyResources.PropertyImageNotFound);

    public static readonly Error InvalidImageFormat = new(
        nameof(PropertyResources.PropertyInvalidImageFormat),
        PropertyResources.PropertyInvalidImageFormat);

    public static readonly Error ImageTooLarge = new(
        nameof(PropertyResources.PropertyImageTooLarge),
        PropertyResources.PropertyImageTooLarge);
}
