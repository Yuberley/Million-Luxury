namespace MillionLuxury.Domain.Owners;

#region Usings
using MillionLuxury.Domain.Abstractions;
#endregion

public interface IOwnerRepository : IRepository<Owner>
{
    Task<Owner?> GetByIdWithPropertiesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Owner> Owners, int TotalCount)> SearchAsync(
        int page,
        int pageSize,
        string? name = null,
        CancellationToken cancellationToken = default);
}
