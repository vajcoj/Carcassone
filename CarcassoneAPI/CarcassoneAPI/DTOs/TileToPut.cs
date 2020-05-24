using CarcassoneAPI.Models;

namespace CarcassoneAPI.DTOs
{
    public class TileToPut
    {
        public int BoardId { get; set; }
        public int TileTypeId { get; set; }
        public TerrainType Top { get; set; }
        public TerrainType Right { get; set; }
        public TerrainType Bottom { get; set; }
        public TerrainType Left { get; set; }
        public string ImageUrl { get; set; }
    }
}
