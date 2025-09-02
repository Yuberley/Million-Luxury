namespace MillionLuxury.Infrastructure.Database.Mappings.Owners;

#region Usings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillionLuxury.Domain.Owners;
#endregion

internal sealed class OwnerMapping : IEntityTypeConfiguration<Owner>
{
    #region Constants
    private const string OwnerTableName = "owners";
    private const int NameMaxLength = 100;
    private const int AddressMaxLength = 50;
    private const int PhotoMaxLength = 500;
    #endregion

    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable(OwnerTableName);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(NameMaxLength);

        builder.OwnsOne(hotel => hotel.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Country).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.State).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.City).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.Street).HasMaxLength(AddressMaxLength);
        });

        builder.Property(o => o.PhotoPath)
            .HasMaxLength(PhotoMaxLength);

        builder.Property(o => o.Birthday)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(o => o.Name);
    }
}
