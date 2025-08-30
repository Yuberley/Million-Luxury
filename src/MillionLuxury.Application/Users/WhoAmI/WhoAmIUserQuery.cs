namespace MillionLuxury.Application.Users.WhoAmI;

using MillionLuxury.Application.Common.Abstractions.CQRS;

public record WhoAmIUserQuery() : IQuery<Dtos.User>;