using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLanders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Lander_LanderId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Lander_Clients_ClientId",
                table: "Lander");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lander",
                table: "Lander");

            migrationBuilder.RenameTable(
                name: "Lander",
                newName: "Landers");

            migrationBuilder.RenameIndex(
                name: "IX_Lander_ClientId",
                table: "Landers",
                newName: "IX_Landers_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Landers",
                table: "Landers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Landers_LanderId",
                table: "Campaigns",
                column: "LanderId",
                principalTable: "Landers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Landers_Clients_ClientId",
                table: "Landers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Landers_LanderId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Landers_Clients_ClientId",
                table: "Landers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Landers",
                table: "Landers");

            migrationBuilder.RenameTable(
                name: "Landers",
                newName: "Lander");

            migrationBuilder.RenameIndex(
                name: "IX_Landers_ClientId",
                table: "Lander",
                newName: "IX_Lander_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lander",
                table: "Lander",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Lander_LanderId",
                table: "Campaigns",
                column: "LanderId",
                principalTable: "Lander",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lander_Clients_ClientId",
                table: "Lander",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
