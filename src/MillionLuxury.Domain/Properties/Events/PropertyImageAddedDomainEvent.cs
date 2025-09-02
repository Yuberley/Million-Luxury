namespace MillionLuxury.Domain.Properties.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record PropertyImageAddedDomainEvent(Guid PropertyId, Guid ImageId) : IDomainEvent;
