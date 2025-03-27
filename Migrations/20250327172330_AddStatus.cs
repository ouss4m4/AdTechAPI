using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Campaigns",
                newName: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Campaigns",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Campaigns",
                newName: "Description");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Campaigns",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
