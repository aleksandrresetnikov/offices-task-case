using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Offices.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOfficeAndPhoneConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_phones_OfficeId",
                table: "phones",
                newName: "IX_Phones_OfficeId");

            migrationBuilder.RenameColumn(
                name: "Coordinates_Longitude",
                table: "offices",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Coordinates_Latitude",
                table: "offices",
                newName: "latitude");

            migrationBuilder.RenameIndex(
                name: "IX_offices_AddressCity_AddressRegion",
                table: "offices",
                newName: "ix_offices_city_region");

            migrationBuilder.AlterColumn<string>(
                name: "Additional",
                table: "phones",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "offices",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "offices",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressStreet",
                table: "offices",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressRegion",
                table: "offices",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressCity",
                table: "offices",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Phones_OfficeId",
                table: "phones",
                newName: "IX_phones_OfficeId");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "offices",
                newName: "Coordinates_Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "offices",
                newName: "Coordinates_Latitude");

            migrationBuilder.RenameIndex(
                name: "ix_offices_city_region",
                table: "offices",
                newName: "IX_offices_AddressCity_AddressRegion");

            migrationBuilder.AlterColumn<string>(
                name: "Additional",
                table: "phones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "offices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "offices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressStreet",
                table: "offices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressRegion",
                table: "offices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressCity",
                table: "offices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}
