namespace MillionLuxury.Domain.Properties.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

public record PropertyPriceChangedDomainEvent(Guid PropertyId, Money OldPrice, Money NewPrice) : IDomainEvent;
