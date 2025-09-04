namespace MillionLuxury.Domain.Properties;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties.Events;
using MillionLuxury.Domain.Properties.ValueObjects;
using MillionLuxury.Domain.PropertyTraces;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

/// <summary>
/// Represents a real estate property in the system
/// </summary>
public sealed class Property : Entity
{
    #region Private Members
    private readonly List<PropertyImage> _images = [];
    private readonly List<PropertyTrace> _traces = [];
    #endregion

    private Property(
        Guid propertyId,
        string name,
        Address address,
        Money price,
        string internalCode,
        int year,
        Guid ownerId,
        PropertyStatus status,
        PropertyDetails details,
        DateTime createdAt
    ) : base(propertyId)
    {
        Name = name;
        Address = address;
        Price = price;
        InternalCode = internalCode;
        Year = year;
        OwnerId = ownerId;
        Status = status;
        Details = details;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Property() { }

    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public string InternalCode { get; private set; }
    public int Year { get; private set; }
    public Guid OwnerId { get; private set; }
    public PropertyStatus Status { get; private set; }
    public PropertyDetails Details { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<PropertyImage> Images => _images.AsReadOnly();
    public IReadOnlyCollection<PropertyTrace> Traces => _traces.AsReadOnly();

    public static Property Create(
        string name,
        Address address,
        Money price,
        string internalCode,
        int year,
        Guid ownerId,
        PropertyDetails details,
        DateTime createdAt
    )
    {
        var property = new Property(
            Guid.NewGuid(),
            name,
            address,
            price,
            internalCode,
            year,
            ownerId,
            PropertyStatus.Available,
            details,
            createdAt
        );

        property.AddDomainEvent(new PropertyCreatedDomainEvent(property.Id));

        return property;
    }

    public void ChangePrice(Money newPrice, DateTime updatedAt)
    {
        if (newPrice.Amount <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(newPrice));

        var oldPrice = Price;
        Price = newPrice;
        UpdatedAt = updatedAt;

        AddDomainEvent(new PropertyPriceChangedDomainEvent(Id, oldPrice, newPrice));
    }

    public void UpdateProperty(
        Guid ownerId,
        string name,
        Address address,
        int year,
        Money price,
        PropertyStatus status,
        PropertyDetails details,
        DateTime updatedAt
    )
    {
        OwnerId = ownerId;
        Name = name;
        Address = address;
        Year = year;
        Price = price;
        Status = status;
        Details = details;
        UpdatedAt = updatedAt;

        AddDomainEvent(new PropertyUpdatedDomainEvent(Id));
    }

    public void AddImage(PropertyImage image)
    {
        _images.Add(image);
        AddDomainEvent(new PropertyImageAddedDomainEvent(Id, image.Id));
    }

    public void RemoveImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(i => i.Id == imageId);
        if (image != null)
        {
            _images.Remove(image);
            AddDomainEvent(new PropertyImageRemovedDomainEvent(Id, imageId));
        }
    }
}
