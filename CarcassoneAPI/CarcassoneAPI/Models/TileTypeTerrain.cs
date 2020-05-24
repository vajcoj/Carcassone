namespace CarcassoneAPI.Models
{
    public class TileTypeTerrain
    {
        //public int Id { get; set; }
        public TilePosition Position { get; set; }
        public TerrainType TerrainType { get; set; }

        public TileTypeComponent TileTypeComponent { get; set; }
        public int TileComponentId { get; set; }

        public TileType TileType { get; set; }
        public int TileTypeId { get; set; }
    }
}