using Microsoft.EntityFrameworkCore.Migrations;

namespace CarcassoneAPI.Migrations
{
    public partial class DontRequireBoardId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TileComponents_BoardComponents_BoardComponentId",
                table: "TileComponents");

            migrationBuilder.AddForeignKey(
                name: "FK_TileComponents_BoardComponents_BoardComponentId",
                table: "TileComponents",
                column: "BoardComponentId",
                principalTable: "BoardComponents",
                principalColumn: "BoardComponentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TileComponents_BoardComponents_BoardComponentId",
                table: "TileComponents");

            migrationBuilder.AddForeignKey(
                name: "FK_TileComponents_BoardComponents_BoardComponentId",
                table: "TileComponents",
                column: "BoardComponentId",
                principalTable: "BoardComponents",
                principalColumn: "BoardComponentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
