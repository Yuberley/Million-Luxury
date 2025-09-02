namespace MillionLuxury.Domain.PropertyTraces;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public interface IPropertyTraceRepository : IRepository<PropertyTrace>
{
    Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(Guid propertyId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<PropertyTrace> Traces, int TotalCount)> SearchAsync(
        int page,
        int pageSize,
        Guid? propertyId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
}
