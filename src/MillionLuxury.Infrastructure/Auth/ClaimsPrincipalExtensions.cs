namespace MillionLuxury.Infrastructure.Auth;

using System.Security.Claims;

internal static class ClaimsPrincipalExtensions
{

    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue("sub")
            ?? principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parseUserId)
            ? parseUserId
            : throw new ApplicationException("User id is unavailable");
    }

}