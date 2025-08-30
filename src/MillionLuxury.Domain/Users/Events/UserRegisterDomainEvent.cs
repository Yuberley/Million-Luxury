namespace MillionLuxury.Domain.User.Events;

using MillionLuxury.Domain.Abstractions;

public record UserRegisterDomainEvent(Guid IdUser) : IDomainEvent;