using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillionLuxury.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerAndPropertyTraceEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "owners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    photo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_owners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "property_traces",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_sale = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    property_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_property_traces", x => x.id);
                    table.ForeignKey(
                        name: "fk_property_traces_properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_properties_owner_id",
                table: "Properties",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_owners_created_at",
                table: "owners",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_owners_name",
                table: "owners",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_property_traces_date_sale",
                table: "property_traces",
                column: "date_sale");

            migrationBuilder.CreateIndex(
                name: "ix_property_traces_property_id",
                table: "property_traces",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "ix_property_traces_property_id_date_sale",
                table: "property_traces",
                columns: new[] { "property_id", "date_sale" });

            migrationBuilder.AddForeignKey(
                name: "fk_properties_owners_owner_id",
                table: "Properties",
                column: "owner_id",
                principalTable: "owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_properties_owners_owner_id",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "owners");

            migrationBuilder.DropTable(
                name: "property_traces");

            migrationBuilder.DropIndex(
                name: "ix_properties_owner_id",
                table: "Properties");
        }
    }
}
