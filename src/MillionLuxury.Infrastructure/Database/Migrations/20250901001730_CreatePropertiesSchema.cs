using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillionLuxury.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreatePropertiesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    internal_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    owner_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    property_type = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    bedrooms = table.Column<int>(type: "int", nullable: false),
                    bathrooms = table.Column<int>(type: "int", nullable: false),
                    area_in_square_meters = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_properties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    property_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    file_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    is_enabled = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_property_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_property_images_property_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_properties_area_in_square_meters",
                table: "Properties",
                column: "area_in_square_meters");

            migrationBuilder.CreateIndex(
                name: "ix_properties_bathrooms",
                table: "Properties",
                column: "bathrooms");

            migrationBuilder.CreateIndex(
                name: "ix_properties_bedrooms",
                table: "Properties",
                column: "bedrooms");

            migrationBuilder.CreateIndex(
                name: "ix_properties_created_at",
                table: "Properties",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_properties_internal_code",
                table: "Properties",
                column: "internal_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_properties_price",
                table: "Properties",
                column: "price");

            migrationBuilder.CreateIndex(
                name: "ix_properties_property_type",
                table: "Properties",
                column: "property_type");

            migrationBuilder.CreateIndex(
                name: "ix_properties_status",
                table: "Properties",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_property_images_is_enabled",
                table: "PropertyImages",
                column: "is_enabled");

            migrationBuilder.CreateIndex(
                name: "ix_property_images_property_id",
                table: "PropertyImages",
                column: "property_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
