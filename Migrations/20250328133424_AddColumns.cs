using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Landers_Clients_ClientId",
                table: "Landers");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Landers",
                newName: "AdvertiserId");

            migrationBuilder.RenameIndex(
                name: "IX_Landers_ClientId",
                table: "Landers",
                newName: "IX_Landers_AdvertiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Landers_Clients_AdvertiserId",
                table: "Landers",
                column: "AdvertiserId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Landers_Clients_AdvertiserId",
                table: "Landers");

            migrationBuilder.RenameColumn(
                name: "AdvertiserId",
                table: "Landers",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Landers_AdvertiserId",
                table: "Landers",
                newName: "IX_Landers_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Landers_Clients_ClientId",
                table: "Landers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
