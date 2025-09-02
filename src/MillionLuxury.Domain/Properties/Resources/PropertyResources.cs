namespace MillionLuxury.Domain.Properties.Resources;

#region Usings
using System.Globalization;
using System.Resources;
#endregion

internal static class PropertyResources
{
    private static readonly ResourceManager ResourceManager = new("MillionLuxury.Domain.Properties.Resources.PropertyResources", typeof(PropertyResources).Assembly);

    internal static string PropertyNotFound => GetString(nameof(PropertyNotFound));
    internal static string PropertyInvalidPrice => GetString(nameof(PropertyInvalidPrice));
    internal static string PropertyInvalidYear => GetString(nameof(PropertyInvalidYear));
    internal static string PropertyInvalidArea => GetString(nameof(PropertyInvalidArea));
    internal static string PropertyInvalidBedrooms => GetString(nameof(PropertyInvalidBedrooms));
    internal static string PropertyInvalidBathrooms => GetString(nameof(PropertyInvalidBathrooms));
    internal static string PropertyInternalCodeAlreadyExists => GetString(nameof(PropertyInternalCodeAlreadyExists));
    internal static string PropertyImageNotFound => GetString(nameof(PropertyImageNotFound));
    internal static string PropertyInvalidImageFormat => GetString(nameof(PropertyInvalidImageFormat));
    internal static string PropertyImageTooLarge => GetString(nameof(PropertyImageTooLarge));

    private static string GetString(string name)
    {
        return ResourceManager.GetString(name, CultureInfo.CurrentCulture) ?? name;
    }
}
