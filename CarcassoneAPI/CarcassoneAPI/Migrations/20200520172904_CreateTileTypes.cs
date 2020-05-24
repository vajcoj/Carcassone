using Microsoft.EntityFrameworkCore.Migrations;

namespace CarcassoneAPI.Migrations
{
    public partial class CreateTileTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    BoardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.BoardId);
                });

            migrationBuilder.CreateTable(
                name: "TileTypes",
                columns: table => new
                {
                    TileTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 128, nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TileTypes", x => x.TileTypeId);
                });

            migrationBuilder.CreateTable(
                name: "BoardComponents",
                columns: table => new
                {
                    BoardComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerrainType = table.Column<int>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    BoardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardComponents", x => x.BoardComponentId);
                    table.ForeignKey(
                        name: "FK_BoardComponents_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "BoardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tiles",
                columns: table => new
                {
                    TileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    Rotation = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    BoardId = table.Column<int>(nullable: false),
                    TileTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiles", x => x.TileId);
                    table.ForeignKey(
                        name: "FK_Tiles_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "BoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tiles_TileTypes_TileTypeId",
                        column: x => x.TileTypeId,
                        principalTable: "TileTypes",
                        principalColumn: "TileTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TileTypeComponents",
                columns: table => new
                {
                    TileTypeComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerrainType = table.Column<int>(nullable: false),
                    TileTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TileTypeComponents", x => x.TileTypeComponentId);
                    table.ForeignKey(
                        name: "FK_TileTypeComponents_TileTypes_TileTypeId",
                        column: x => x.TileTypeId,
                        principalTable: "TileTypes",
                        principalColumn: "TileTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TileComponents",
                columns: table => new
                {
                    TileTypeComponentId = table.Column<int>(nullable: false),
                    TileId = table.Column<int>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    BoardComponentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TileComponents", x => new { x.TileId, x.TileTypeComponentId });
                    table.ForeignKey(
                        name: "FK_TileComponents_BoardComponents_BoardComponentId",
                        column: x => x.BoardComponentId,
                        principalTable: "BoardComponents",
                        principalColumn: "BoardComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TileComponents_Tiles_TileId",
                        column: x => x.TileId,
                        principalTable: "Tiles",
                        principalColumn: "TileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TileComponents_TileTypeComponents_TileTypeComponentId",
                        column: x => x.TileTypeComponentId,
                        principalTable: "TileTypeComponents",
                        principalColumn: "TileTypeComponentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TileTypeTerrains",
                columns: table => new
                {
                    Position = table.Column<int>(nullable: false),
                    TileTypeId = table.Column<int>(nullable: false),
                    TerrainType = table.Column<int>(nullable: false),
                    TileTypeComponentId = table.Column<int>(nullable: true),
                    TileComponentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TileTypeTerrains", x => new { x.TileTypeId, x.Position });
                    table.ForeignKey(
                        name: "FK_TileTypeTerrains_TileTypeComponents_TileTypeComponentId",
                        column: x => x.TileTypeComponentId,
                        principalTable: "TileTypeComponents",
                        principalColumn: "TileTypeComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TileTypeTerrains_TileTypes_TileTypeId",
                        column: x => x.TileTypeId,
                        principalTable: "TileTypes",
                        principalColumn: "TileTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardComponents_BoardId",
                table: "BoardComponents",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TileComponents_BoardComponentId",
                table: "TileComponents",
                column: "BoardComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_TileComponents_TileTypeComponentId",
                table: "TileComponents",
                column: "TileTypeComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_TileTypeId",
                table: "Tiles",
                column: "TileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tiles_BoardId_X_Y",
                table: "Tiles",
                columns: new[] { "BoardId", "X", "Y" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TileTypeComponents_TileTypeId",
                table: "TileTypeComponents",
                column: "TileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TileTypeTerrains_TileTypeComponentId",
                table: "TileTypeTerrains",
                column: "TileTypeComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TileComponents");

            migrationBuilder.DropTable(
                name: "TileTypeTerrains");

            migrationBuilder.DropTable(
                name: "BoardComponents");

            migrationBuilder.DropTable(
                name: "Tiles");

            migrationBuilder.DropTable(
                name: "TileTypeComponents");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "TileTypes");
        }
    }
}
