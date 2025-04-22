using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class countryFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Countries",
                newName: "Iso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Iso",
                table: "Countries",
                newName: "Code");
        }
    }
}
