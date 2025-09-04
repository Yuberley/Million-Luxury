namespace MillionLuxury.Infrastructure.Database.Repositories;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Domain.Owners;
using MillionLuxury.Infrastructure.Repositories;
#endregion

internal sealed class OwnerRepository : Repository<Owner>, IOwnerRepository
{
    public OwnerRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    public async Task<Owner?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Owner>()
            .FirstOrDefaultAsync(o => o.Name == name, cancellationToken);
    }

    public async Task<Owner?> GetByIdWithPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Owner>()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Owner>()
            .AnyAsync(o => o.Name == name, cancellationToken);
    }

    public async Task<(IEnumerable<Owner> Owners, int TotalCount)> SearchAsync(
        int pageNumber, 
        int pageSize, 
        string? nameFilter, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Owner>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(nameFilter))
        {
            query = query.Where(o => o.Name.Contains(nameFilter));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
