using Microsoft.EntityFrameworkCore.Migrations;

namespace CarcassoneAPI.Migrations
{
    public partial class pok : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BoardComponentId",
                table: "TileComponents",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BoardComponentId",
                table: "TileComponents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
