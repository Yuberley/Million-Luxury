namespace MillionLuxury.Domain.Users;

using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users.Resources;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        nameof(UserResources.UserNotFound),
        UserResources.UserNotFound);

    public static readonly Error InvalidCredentials = new(
        nameof(UserResources.InvalidCredentials),
        UserResources.InvalidCredentials);

    public static Error EmailAlreadyExists(string email) => new(
        nameof(UserResources.EmailAlreadyExists),
        string.Format(UserResources.EmailAlreadyExists, email));

    public static Error RolesDoNotExist(string[] roles) => new(
        nameof(UserResources.RolesDoNotExist),
        string.Format(UserResources.RolesDoNotExist, string.Join(", ", roles)));
}