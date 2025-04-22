using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class renameCountryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "NiceName",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NiceName",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Countries",
                type: "text",
                nullable: true);
        }
    }
}
