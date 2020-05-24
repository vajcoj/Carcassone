using CarcassoneAPI.Models;

namespace CarcassoneAPI.DTOs
{
    public class TilePutted
    {
        public int TileTypeId { get; set; }
        public int Rotation { get; set; }
        public int BoardId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string ImageUrl { get; set; }
        public TerrainType Top { get; set; }
        public TerrainType Right { get; set; }
        public TerrainType Bottom { get; set; }
        public TerrainType Left { get; set; }
    }
}
