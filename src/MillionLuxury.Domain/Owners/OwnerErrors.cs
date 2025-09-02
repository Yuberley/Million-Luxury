namespace MillionLuxury.Domain.Owners;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners.Resources;
#endregion

public static class OwnerErrors
{
    public static readonly Error NotFound = new(
        nameof(OwnerResources.OwnerNotFound),
        OwnerResources.OwnerNotFound);

    public static readonly Error AlreadyExists = new(
        nameof(OwnerResources.OwnerAlreadyExists),
        OwnerResources.OwnerAlreadyExists);

    public static Error NameAlreadyExists(string name) => new(
        nameof(OwnerResources.OwnerNameAlreadyExists),
        string.Format(OwnerResources.OwnerNameAlreadyExists, name));

    public static readonly Error CannotBeDeleted = new(
        nameof(OwnerResources.OwnerCannotBeDeleted),
        OwnerResources.OwnerCannotBeDeleted);

    public static readonly Error NameRequired = new(
        nameof(OwnerResources.OwnerNameRequired),
        OwnerResources.OwnerNameRequired);

    public static readonly Error NameMaxLength = new(
        nameof(OwnerResources.OwnerNameMaxLength),
        OwnerResources.OwnerNameMaxLength);

    public static readonly Error AddressRequired = new(
        nameof(OwnerResources.OwnerAddressRequired),
        OwnerResources.OwnerAddressRequired);

    public static readonly Error AddressMaxLength = new(
        nameof(OwnerResources.OwnerAddressMaxLength),
        OwnerResources.OwnerAddressMaxLength);

    public static readonly Error BirthdayRequired = new(
        nameof(OwnerResources.OwnerBirthdayRequired),
        OwnerResources.OwnerBirthdayRequired);

    public static readonly Error BirthdayInvalidDate = new(
        nameof(OwnerResources.OwnerBirthdayInvalidDate),
        OwnerResources.OwnerBirthdayInvalidDate);

    public static readonly Error PhotoTooLarge = new(
        nameof(OwnerResources.OwnerPhotoTooLarge),
        OwnerResources.OwnerPhotoTooLarge);

    public static readonly Error InvalidPhotoFormat = new(
        nameof(OwnerResources.OwnerInvalidPhotoFormat),
        OwnerResources.OwnerInvalidPhotoFormat);
}
