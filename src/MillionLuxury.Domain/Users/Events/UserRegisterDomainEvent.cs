namespace MillionLuxury.Domain.Users.Events;

using MillionLuxury.Domain.Abstractions;

public record UserRegisterDomainEvent(Guid IdUser) : IDomainEvent;