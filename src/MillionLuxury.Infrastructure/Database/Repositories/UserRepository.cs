namespace MillionLuxury.Infrastructure.Database.Repositories;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Domain.Users;
using MillionLuxury.Infrastructure.Repositories; 
#endregion

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }
}
