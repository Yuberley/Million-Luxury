using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillionLuxury.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamePropertyImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PropertyImages",
                newName: "property_images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "property_images",
                newName: "PropertyImages");
        }
    }
}
