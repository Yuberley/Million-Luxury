namespace MillionLuxury.Domain.Owners;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners.Events;
using MillionLuxury.Domain.Properties.ValueObjects;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

/// <summary>
/// Represents a property owner in the system
/// </summary>
public sealed class Owner : Entity
{
    #region Constants
    private const int MAX_NAME_LENGTH = 200;
    private const int MAX_ADDRESS_LENGTH = 500;
    private const int MAX_PHOTO_PATH_LENGTH = 1000;
    #endregion

    private Owner(
        Guid ownerId,
        string name,
        Address address,
        string? photoPath,
        DateTime birthday,
        DateTime createdAt
    ) : base(ownerId)
    {
        Name = name;
        Address = address;
        PhotoPath = photoPath;
        Birthday = birthday;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Owner() { }

    public string Name { get; private set; }
    public Address Address { get; private set; }
    public string? PhotoPath { get; private set; }
    public DateTime Birthday { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static Owner Create(
        string name,
        Address address,
        string? photoPath,
        DateTime birthday,
        DateTime createdAt
    )
    {
        var owner = new Owner(
            Guid.NewGuid(),
            name,
            address,
            photoPath,
            birthday,
            createdAt
        );

        owner.AddDomainEvent(new OwnerCreatedDomainEvent(owner.Id));

        return owner;
    }

    public void UpdateOwner(
        string name,
        Address address,
        DateTime birthday,
        DateTime updatedAt
    )
    {
        Name = name;
        Address = address;
        Birthday = birthday;
        UpdatedAt = updatedAt;

        AddDomainEvent(new OwnerUpdatedDomainEvent(Id));
    }

    public void UpdatePhoto(string? photoPath, DateTime updatedAt)
    {
        PhotoPath = photoPath;
        UpdatedAt = updatedAt;

        AddDomainEvent(new OwnerPhotoUpdatedDomainEvent(Id));
    }
}
