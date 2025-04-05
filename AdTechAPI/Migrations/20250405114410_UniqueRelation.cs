using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class UniqueRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RollupHour_StatDate_StatHour_PublisherId_TrafficSourceId_Ad~",
                table: "RollupHour",
                columns: new[] { "StatDate", "StatHour", "PublisherId", "TrafficSourceId", "AdvertiserId", "CampaignId", "LanderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RollupHour_StatDate_StatHour_PublisherId_TrafficSourceId_Ad~",
                table: "RollupHour");
        }
    }
}
