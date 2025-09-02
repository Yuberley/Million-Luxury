namespace MillionLuxury.Domain.Properties.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record PropertyUpdatedDomainEvent(Guid PropertyId) : IDomainEvent;
