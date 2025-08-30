namespace MillionLuxury.Application.Users;

using MillionLuxury.Domain.Users;

internal static class UserExtensions
{
    public static Dtos.User ToDto(this User user)
    {
        return new Dtos.User(
            user.Id,
            user.Email,
            user.Roles.Values.ToArray(),
            user.IsEmailVerified,
            user.CreatedAt
        );
    }
}
