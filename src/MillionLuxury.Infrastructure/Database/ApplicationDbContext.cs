namespace MillionLuxury.Infrastructure.Database;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Application.Common.Exceptions;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Owners;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.PropertyTraces;
using MillionLuxury.Domain.Users;
#endregion

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<PropertyTrace> PropertyTraces { get; set; }

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

        modelBuilder.Entity<Property>()
            .Navigation(p => p.Images)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<Property>()
            .Navigation(p => p.Traces)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        base.OnModelCreating(modelBuilder);
    }
}
