namespace MillionLuxury.Domain.Owners.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record OwnerCreatedDomainEvent(Guid OwnerId) : IDomainEvent;
