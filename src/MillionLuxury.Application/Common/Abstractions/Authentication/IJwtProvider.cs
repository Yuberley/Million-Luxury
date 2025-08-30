namespace MillionLuxury.Application.Common.Abstractions.Authentication;

using MillionLuxury.Domain.User;

public interface IJwtProvider
{
    string GenerateToken(User user);
}