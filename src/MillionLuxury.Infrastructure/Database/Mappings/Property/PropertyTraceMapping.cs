namespace MillionLuxury.Infrastructure.Database.Mappings.Property;

#region Usings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.PropertyTraces;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

internal sealed class PropertyTraceMapping : IEntityTypeConfiguration<PropertyTrace>
{
    #region Constants
    private const string PropertyTraceTableName = "property_traces";
    private const int NameMaxLength = 200;
    private const string PriceColumnType = "decimal(18,2)";
    private const string TaxColumnType = "decimal(18,2)";
    #endregion

    public void Configure(EntityTypeBuilder<PropertyTrace> builder)
    {
        builder.ToTable(PropertyTraceTableName);

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.PropertyId).IsRequired();
        builder.Property(pt => pt.DateSale).IsRequired();
        builder.Property(pt => pt.Name).IsRequired().HasMaxLength(NameMaxLength);
        builder.OwnsOne(pt => pt.Value, valueBuilder =>
        {
            valueBuilder.Property(v => v.Amount)
              .HasColumnType(PriceColumnType)
              .IsRequired();

            valueBuilder.Property(money => money.Currency)
                .HasConversion(
                    currency => currency.Code,
                    code => Currency.FromCode(code));
        });

        builder.Property(booking => booking.Tax)
            .HasConversion(
                taxes => taxes.Value,
                value => Taxes.FromValue(value))
            .HasColumnType(TaxColumnType);

        builder.Property(pt => pt.CreatedAt).IsRequired();

        builder.HasIndex(pt => pt.PropertyId);

        builder.HasOne<Property>()
            .WithMany(p => p.Traces)
            .HasForeignKey(propertyTrace => propertyTrace.PropertyId)
            .HasPrincipalKey(property => property.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
