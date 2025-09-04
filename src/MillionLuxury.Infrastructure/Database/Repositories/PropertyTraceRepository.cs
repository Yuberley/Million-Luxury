namespace MillionLuxury.Infrastructure.Database.Repositories;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Domain.PropertyTraces;
using MillionLuxury.Infrastructure.Repositories;
#endregion

internal sealed class PropertyTraceRepository : Repository<PropertyTrace>, IPropertyTraceRepository
{
    public PropertyTraceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {

    }

    public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<PropertyTrace>()
            .Where(pt => pt.PropertyId == propertyId)
            .OrderByDescending(pt => pt.DateSale)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<PropertyTrace> Traces, int TotalCount)> SearchAsync(
        int page,
        int pageSize,
        Guid? propertyId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<PropertyTrace>().AsQueryable();

        if (propertyId.HasValue)
        {
            query = query.Where(pt => pt.PropertyId == propertyId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(pt => pt.DateSale >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(pt => pt.DateSale <= toDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var traces = await query
            .OrderByDescending(pt => pt.DateSale)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (traces, totalCount);
    }
}
