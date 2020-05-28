using Microsoft.EntityFrameworkCore.Migrations;

namespace CarcassoneAPI.Migrations
{
    public partial class AddTerrainTypeToTileComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TerrainType",
                table: "TileComponents",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TerrainType",
                table: "TileComponents");
        }
    }
}
