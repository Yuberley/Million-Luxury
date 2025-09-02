namespace MillionLuxury.Infrastructure.Database.Mappings.Property;

#region Usings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillionLuxury.Domain.Owners;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

internal sealed class PropertyMapping : IEntityTypeConfiguration<Property>
{
    #region Constants
    private const string PropertyTableName = "properties";
    private const int NameMaxLength = 200;
    private const int AddressMaxLength = 50;
    private const int InternalCodeMaxLength = 50;
    private const int DescriptionMaxLength = 2000;
    private const string PriceColumnType = "decimal(18,2)";
    #endregion

    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable(PropertyTableName);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(NameMaxLength);
        builder.OwnsOne(hotel => hotel.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Country).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.State).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.City).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(AddressMaxLength);
            addressBuilder.Property(a => a.Street).HasMaxLength(AddressMaxLength);
        });

        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(v => v.Amount)
              .HasColumnType(PriceColumnType)
              .IsRequired();

            priceBuilder.Property(money => money.Currency)
                .HasConversion(
                    currency => currency.Code,
                    code => Currency.FromCode(code));
        });

        builder.Property(p => p.InternalCode).IsRequired().HasMaxLength(InternalCodeMaxLength);
        builder.HasIndex(p => p.InternalCode).IsUnique();
        builder.Property(p => p.Year).IsRequired();
        builder.Property(p => p.OwnerId).IsRequired();
        builder.Property(p => p.Status).IsRequired().HasConversion(
            status => status.ToString(),
            value => Enum.Parse<Domain.Properties.ValueObjects.PropertyStatus>(value)
        );

        builder.OwnsOne(p => p.Details, detailsBuilder =>
        {
            detailsBuilder.Property(d => d.PropertyType).IsRequired().HasConversion<int>();
            detailsBuilder.Property(d => d.Bedrooms).IsRequired();
            detailsBuilder.Property(d => d.Bathrooms).IsRequired();
            detailsBuilder.Property(d => d.AreaInSquareMeters).IsRequired().HasColumnType("decimal(10,2)");
            detailsBuilder.Property(d => d.Description).IsRequired().HasMaxLength(DescriptionMaxLength);
        });

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasIndex(p => p.InternalCode);

        builder.HasOne<Owner>()
            .WithMany()
            .HasForeignKey(property => property.OwnerId)
            .HasPrincipalKey(owner => owner.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
