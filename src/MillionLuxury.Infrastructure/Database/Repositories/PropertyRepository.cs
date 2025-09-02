namespace MillionLuxury.Infrastructure.Database.Repositories;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Properties.ValueObjects;
using MillionLuxury.Infrastructure.Database;
using MillionLuxury.Infrastructure.Repositories;
#endregion

internal sealed class PropertyRepository : Repository<Property>, IPropertyRepository
{
    public PropertyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Property?> GetByIdWithImagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Property>()
            .Include("_images")
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByInternalCodeAsync(string internalCode, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Property>()
            .AnyAsync(p => p.InternalCode == internalCode, cancellationToken);
    }

    public async Task<bool> ExistsByInternalCodeAsync(string internalCode, Guid excludePropertyId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Property>()
            .AnyAsync(p => p.InternalCode == internalCode && p.Id != excludePropertyId, cancellationToken);
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> SearchAsync(
        int page,
        int pageSize,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        PropertyStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Property>().AsQueryable();

        // Apply filters
        if (minPrice.HasValue)
            query = query.Where(p => p.Price.Amount >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price.Amount <= maxPrice.Value);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var properties = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include("_images")
            .ToListAsync(cancellationToken);

        return (properties, totalCount);
    }
}
