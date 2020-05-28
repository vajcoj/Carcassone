using Microsoft.EntityFrameworkCore.Migrations;

namespace CarcassoneAPI.Migrations
{
    public partial class SetRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TileTypeTerrains",
                table: "TileTypeTerrains");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TileComponents",
                table: "TileComponents");

            migrationBuilder.AddColumn<int>(
                name: "TileTypeTerrainId",
                table: "TileTypeTerrains",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "BoardComponentId",
                table: "TileComponents",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TileComponentId",
                table: "TileComponents",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TileTypeTerrains",
                table: "TileTypeTerrains",
                column: "TileTypeTerrainId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TileComponents",
                table: "TileComponents",
                column: "TileComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_TileTypeTerrains_TileTypeId",
                table: "TileTypeTerrains",
                column: "TileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TileComponents_TileId",
                table: "TileComponents",
                column: "TileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TileTypeTerrains",
                table: "TileTypeTerrains");

            migrationBuilder.DropIndex(
                name: "IX_TileTypeTerrains_TileTypeId",
                table: "TileTypeTerrains");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TileComponents",
                table: "TileComponents");

            migrationBuilder.DropIndex(
                name: "IX_TileComponents_TileId",
                table: "TileComponents");

            migrationBuilder.DropColumn(
                name: "TileTypeTerrainId",
                table: "TileTypeTerrains");

            migrationBuilder.DropColumn(
                name: "TileComponentId",
                table: "TileComponents");

            migrationBuilder.AlterColumn<int>(
                name: "BoardComponentId",
                table: "TileComponents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TileTypeTerrains",
                table: "TileTypeTerrains",
                columns: new[] { "TileTypeId", "Position" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TileComponents",
                table: "TileComponents",
                columns: new[] { "TileId", "TileTypeComponentId" });
        }
    }
}
