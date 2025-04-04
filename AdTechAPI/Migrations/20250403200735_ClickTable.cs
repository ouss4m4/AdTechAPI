using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class ClickTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clicks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    PublisherId = table.Column<int>(type: "integer", nullable: false),
                    TrafficSourceId = table.Column<int>(type: "integer", nullable: false),
                    AdvertiserId = table.Column<int>(type: "integer", nullable: false),
                    CampaignId = table.Column<int>(type: "integer", nullable: false),
                    LanderId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clicks_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clicks_Clients_AdvertiserId",
                        column: x => x.AdvertiserId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clicks_Clients_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clicks_Landers_LanderId",
                        column: x => x.LanderId,
                        principalTable: "Landers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clicks_TrafficSources_TrafficSourceId",
                        column: x => x.TrafficSourceId,
                        principalTable: "TrafficSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_AdvertiserId",
                table: "Clicks",
                column: "AdvertiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_CampaignId",
                table: "Clicks",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_LanderId",
                table: "Clicks",
                column: "LanderId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_PublisherId",
                table: "Clicks",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Clicks_TrafficSourceId",
                table: "Clicks",
                column: "TrafficSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clicks");
        }
    }
}
