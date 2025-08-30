namespace MillionLuxury.Infrastructure.Database;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Exceptions;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users;
#endregion

public sealed class ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : DbContext(options), IUnitOfWork
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
