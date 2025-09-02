namespace MillionLuxury.Domain.PropertyTraces;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.PropertyTraces.Events;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

/// <summary>
/// Represents a property transaction trace in the system
/// </summary>
public sealed class PropertyTrace : Entity
{
    private PropertyTrace(
        Guid propertyTraceId,
        DateTime dateSale,
        string name,
        Money value,
        Taxes tax,
        Guid propertyId,
        DateTime createdAt
    ) : base(propertyTraceId)
    {
        DateSale = dateSale;
        Name = name;
        Value = value;
        Tax = tax;
        PropertyId = propertyId;
        CreatedAt = createdAt;
    }

    private PropertyTrace() { }

    public DateTime DateSale { get; private set; }
    public string Name { get; private set; }
    public Money Value { get; private set; }
    public Taxes Tax { get; private set; }
    public Guid PropertyId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static PropertyTrace Create(
        DateTime dateSale,
        string name,
        Money value,
        Taxes tax,
        Guid propertyId,
        DateTime createdAt
    )
    {
        var propertyTrace = new PropertyTrace(
            Guid.NewGuid(),
            dateSale,
            name,
            value,
            tax,
            propertyId,
            createdAt
        );

        propertyTrace.AddDomainEvent(new PropertyTraceCreatedDomainEvent(propertyTrace.Id, propertyId));

        return propertyTrace;
    }

    public void UpdateTrace(
        DateTime dateSale,
        string name,
        Money value,
        Taxes tax
    )
    {
        DateSale = dateSale;
        Name = name;
        Value = value;
        Tax = tax;

        AddDomainEvent(new PropertyTraceUpdatedDomainEvent(Id, PropertyId));
    }
}
