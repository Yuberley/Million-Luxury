namespace MillionLuxury.Domain.Properties;

#region Usings
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties.ValueObjects;
#endregion

public interface IPropertyRepository : IRepository<Property>
{
    Task<Property?> GetByIdWithImagesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByInternalCodeAsync(string internalCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByInternalCodeAsync(string internalCode, Guid excludePropertyId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Property> Properties, int TotalCount)> SearchAsync(
        int page,
        int pageSize,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        PropertyStatus? status = null,
        CancellationToken cancellationToken = default);
}
