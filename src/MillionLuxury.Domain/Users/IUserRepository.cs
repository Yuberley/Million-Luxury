namespace MillionLuxury.Domain.Users;

using MillionLuxury.Domain.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}