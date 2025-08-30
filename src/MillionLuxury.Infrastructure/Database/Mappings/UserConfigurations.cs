namespace MillionLuxury.Infrastructure.Database.Configurations;

#region Usings
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MillionLuxury.Domain.Users;
using MillionLuxury.Domain.Users.ValueObjects;
#endregion

internal sealed class UserConfigurations : IEntityTypeConfiguration<User>
{
    #region Constants
    private const string TableName = "users";
    private const int MaxEmailLength = 150;
    private const int MaxPasswordLength = 255;
    private const string DelimitingCharacter = ",";
    #endregion

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email).HasMaxLength(MaxEmailLength);
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Property(user => user.PasswordHash).HasMaxLength(MaxPasswordLength);
        builder.Property(user => user.IsEmailVerified);
        builder.Property(user => user.CreatedAt);
        builder.Property(user => user.Roles)
            .HasConversion(
                roles => string.Join(DelimitingCharacter, roles.Values),
                value => Roles.Create(value.Split(DelimitingCharacter, StringSplitOptions.None)));

        builder.HasIndex(user => user.Email).IsUnique();
    }
}