namespace MillionLuxury.Infrastructure.Database;

#region Usings
using Microsoft.EntityFrameworkCore;
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Common.Exceptions;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Users;
using MillionLuxury.Domain.Owners;
using MillionLuxury.Domain.PropertyTraces;
#endregion

public sealed class ApplicationDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : DbContext(options), IUnitOfWork
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

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

        // Configure Property -> PropertyImage relationship using the backing field
        //modelBuilder.Entity<Property>()
        //    .HasMany(p => p.Images)
        //    .WithOne()
        //    .HasForeignKey(pi => pi.PropertyId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // Configure the backing field for the Images navigation property
        modelBuilder.Entity<Property>()
            .Navigation(p => p.Images)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Configure Property -> PropertyTrace relationship using the backing field
        //modelBuilder.Entity<Property>()
        //    .HasMany(p => p.Traces)
        //    .WithOne()
        //    .HasForeignKey(pt => pt.PropertyId)
        //    .OnDelete(DeleteBehavior.Cascade);

        // Configure the backing field for the Traces navigation property
        modelBuilder.Entity<Property>()
            .Navigation(p => p.Traces)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        base.OnModelCreating(modelBuilder);
    }
}
