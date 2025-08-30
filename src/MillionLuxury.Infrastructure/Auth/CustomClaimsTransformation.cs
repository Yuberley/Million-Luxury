namespace MillionLuxury.Infrastructure.Auth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

internal sealed class CustomClaimsTransformation : IClaimsTransformation
{

    private readonly IServiceProvider _serviceProvider;

    public CustomClaimsTransformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == "sub"))
        {
            return principal;
        }

        using var scope = _serviceProvider.CreateScope();

        var identityId = principal.GetUserId();

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, identityId.ToString()));

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}