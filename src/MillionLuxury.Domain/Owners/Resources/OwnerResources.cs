namespace MillionLuxury.Domain.Owners.Resources;

#region Usings
using System.Globalization;
using System.Resources;
#endregion

internal static class OwnerResources
{
    private static readonly ResourceManager ResourceManager = new("MillionLuxury.Domain.Owners.Resources.OwnerResources", typeof(OwnerResources).Assembly);

    internal static string OwnerNameRequired => GetString(nameof(OwnerNameRequired));
    internal static string OwnerNameMaxLength => GetString(nameof(OwnerNameMaxLength));
    internal static string OwnerAddressRequired => GetString(nameof(OwnerAddressRequired));
    internal static string OwnerAddressMaxLength => GetString(nameof(OwnerAddressMaxLength));
    internal static string OwnerBirthdayRequired => GetString(nameof(OwnerBirthdayRequired));
    internal static string OwnerBirthdayInvalidDate => GetString(nameof(OwnerBirthdayInvalidDate));
    internal static string OwnerNotFound => GetString(nameof(OwnerNotFound));
    internal static string OwnerAlreadyExists => GetString(nameof(OwnerAlreadyExists));
    internal static string OwnerNameAlreadyExists => GetString(nameof(OwnerNameAlreadyExists));
    internal static string OwnerCannotBeDeleted => GetString(nameof(OwnerCannotBeDeleted));
    internal static string OwnerCreatedSuccessfully => GetString(nameof(OwnerCreatedSuccessfully));
    internal static string OwnerUpdatedSuccessfully => GetString(nameof(OwnerUpdatedSuccessfully));
    internal static string OwnerDeletedSuccessfully => GetString(nameof(OwnerDeletedSuccessfully));
    internal static string OwnerPhotoTooLarge => GetString(nameof(OwnerPhotoTooLarge));
    internal static string OwnerInvalidPhotoFormat => GetString(nameof(OwnerInvalidPhotoFormat));

    private static string GetString(string name)
    {
        return ResourceManager.GetString(name, CultureInfo.CurrentCulture) ?? name;
    }
}
