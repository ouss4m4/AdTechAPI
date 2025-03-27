using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class campaignupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<int>>(
                name: "Platforms",
                table: "Campaigns",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<int>>(
                name: "Countries",
                table: "Campaigns",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Platforms",
                table: "Campaigns",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Countries",
                table: "Campaigns",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "jsonb");
        }
    }
}
