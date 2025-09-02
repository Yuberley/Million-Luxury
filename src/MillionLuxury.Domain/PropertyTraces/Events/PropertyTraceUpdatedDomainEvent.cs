namespace MillionLuxury.Domain.PropertyTraces.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record PropertyTraceUpdatedDomainEvent(Guid PropertyTraceId, Guid PropertyId) : IDomainEvent;
