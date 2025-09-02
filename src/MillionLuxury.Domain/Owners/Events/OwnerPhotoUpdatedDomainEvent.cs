namespace MillionLuxury.Domain.Owners.Events;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public record OwnerPhotoUpdatedDomainEvent(Guid OwnerId) : IDomainEvent;
