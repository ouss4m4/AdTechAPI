using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class whatever : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rollups",
                table: "Rollups");

            migrationBuilder.RenameTable(
                name: "Rollups",
                newName: "ClicksRollupHourly");

            migrationBuilder.AddColumn<DateOnly>(
                name: "RollupDate",
                table: "ClicksRollupHourly",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateTime>(
                name: "RollupHour",
                table: "ClicksRollupHourly",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClicksRollupHourly",
                table: "ClicksRollupHourly",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClicksRollupHourly",
                table: "ClicksRollupHourly");

            migrationBuilder.DropColumn(
                name: "RollupDate",
                table: "ClicksRollupHourly");

            migrationBuilder.DropColumn(
                name: "RollupHour",
                table: "ClicksRollupHourly");

            migrationBuilder.RenameTable(
                name: "ClicksRollupHourly",
                newName: "Rollups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rollups",
                table: "Rollups",
                column: "Id");
        }
    }
}
