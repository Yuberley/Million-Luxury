namespace MillionLuxury.Domain.PropertyTraces.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record PropertyTraceCreatedDomainEvent(Guid PropertyTraceId, Guid PropertyId) : IDomainEvent;
