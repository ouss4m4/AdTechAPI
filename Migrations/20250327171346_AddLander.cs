using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLander : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "LanderId",
                table: "Campaigns",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lander",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lander", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lander_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_LanderId",
                table: "Campaigns",
                column: "LanderId");

            migrationBuilder.CreateIndex(
                name: "IX_Lander_ClientId",
                table: "Lander",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Lander_LanderId",
                table: "Campaigns",
                column: "LanderId",
                principalTable: "Lander",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Lander_LanderId",
                table: "Campaigns");

            migrationBuilder.DropTable(
                name: "Lander");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_LanderId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "LanderId",
                table: "Campaigns");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Campaigns",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Campaigns",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
