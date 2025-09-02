using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillionLuxury.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseImprovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_property_traces_date_sale",
                table: "property_traces");

            migrationBuilder.DropIndex(
                name: "ix_property_traces_property_id_date_sale",
                table: "property_traces");

            migrationBuilder.DropIndex(
                name: "ix_properties_area_in_square_meters",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_properties_bathrooms",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_properties_bedrooms",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_properties_created_at",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_properties_price",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_properties_property_type",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_properties_status",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "ix_owners_created_at",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "address",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "address",
                table: "owners");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "properties");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "property_traces",
                newName: "value_amount");

            migrationBuilder.RenameColumn(
                name: "property_type",
                table: "properties",
                newName: "details_property_type");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "properties",
                newName: "details_description");

            migrationBuilder.RenameColumn(
                name: "bedrooms",
                table: "properties",
                newName: "details_bedrooms");

            migrationBuilder.RenameColumn(
                name: "bathrooms",
                table: "properties",
                newName: "details_bathrooms");

            migrationBuilder.RenameColumn(
                name: "area_in_square_meters",
                table: "properties",
                newName: "details_area_in_square_meters");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "properties",
                newName: "price_amount");

            migrationBuilder.RenameColumn(
                name: "photo",
                table: "owners",
                newName: "photo_path");

            migrationBuilder.AddColumn<string>(
                name: "value_currency",
                table: "property_traces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "properties",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "address_city",
                table: "properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_country",
                table: "properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_state",
                table: "properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_street",
                table: "properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_zip_code",
                table: "properties",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "price_currency",
                table: "properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_city",
                table: "owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_country",
                table: "owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_state",
                table: "owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_street",
                table: "owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address_zip_code",
                table: "owners",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "value_currency",
                table: "property_traces");

            migrationBuilder.DropColumn(
                name: "address_city",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "address_country",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "address_state",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "address_street",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "address_zip_code",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "price_currency",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "address_city",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "address_country",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "address_state",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "address_street",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "address_zip_code",
                table: "owners");

            migrationBuilder.RenameTable(
                name: "properties",
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "value_amount",
                table: "property_traces",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "details_property_type",
                table: "Properties",
                newName: "property_type");

            migrationBuilder.RenameColumn(
                name: "details_description",
                table: "Properties",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "details_bedrooms",
                table: "Properties",
                newName: "bedrooms");

            migrationBuilder.RenameColumn(
                name: "details_bathrooms",
                table: "Properties",
                newName: "bathrooms");

            migrationBuilder.RenameColumn(
                name: "details_area_in_square_meters",
                table: "Properties",
                newName: "area_in_square_meters");

            migrationBuilder.RenameColumn(
                name: "price_amount",
                table: "Properties",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "photo_path",
                table: "owners",
                newName: "photo");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Properties",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Properties",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "owners",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_property_traces_date_sale",
                table: "property_traces",
                column: "date_sale");

            migrationBuilder.CreateIndex(
                name: "ix_property_traces_property_id_date_sale",
                table: "property_traces",
                columns: new[] { "property_id", "date_sale" });

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
                name: "ix_owners_created_at",
                table: "owners",
                column: "created_at");
        }
    }
}
