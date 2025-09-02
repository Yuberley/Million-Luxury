namespace MillionLuxury.Domain.Properties.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record PropertyCreatedDomainEvent(Guid PropertyId) : IDomainEvent;
