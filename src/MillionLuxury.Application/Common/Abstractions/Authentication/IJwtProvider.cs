namespace MillionLuxury.Application.Common.Abstractions.Authentication;

using MillionLuxury.Domain.Users;

public interface IJwtProvider
{
    string GenerateToken(User user);
}