namespace MillionLuxury.Infrastructure.Database.Mappings.Property;

#region Usings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillionLuxury.Domain.Properties;
#endregion

internal sealed class PropertyImageMapping : IEntityTypeConfiguration<PropertyImage>
{
    #region Constants
    private const string PropertyImageTableName = "property_images";
    private const int FileNameMaxLength = 255;
    private const int FilePathMaxLength = 500;
    #endregion

    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.ToTable(PropertyImageTableName);

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.PropertyId).IsRequired();
        builder.Property(pi => pi.FileName).IsRequired().HasMaxLength(FileNameMaxLength);
        builder.Property(pi => pi.FilePath).IsRequired().HasMaxLength(FilePathMaxLength);
        builder.Property(pi => pi.IsEnabled).IsRequired();
        builder.Property(pi => pi.CreatedAt).IsRequired();

        builder.HasIndex(pi => pi.PropertyId);
        builder.HasIndex(pi => pi.IsEnabled);

        builder.HasOne<Property>()
            .WithMany(p => p.Images)
            .HasForeignKey(propertyImage => propertyImage.PropertyId)
            .HasPrincipalKey(property => property.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
