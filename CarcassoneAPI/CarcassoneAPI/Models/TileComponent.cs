namespace CarcassoneAPI.Models
{
    /// <summary>
    /// List of connected terrains of same terrain type for one particular Tile
    /// </summary>
    public class TileComponent
    {
        public int TileComponentId { get; set; }
        public bool IsOpen { get; set; }
        public TerrainType TerrainType { get; set; }
        public TileTypeComponent TileTypeComponent { get; set; }
        public int TileTypeComponentId { get; set; }
        public Tile Tile { get; set; }
        public int TileId { get; set; }
        public BoardComponent BoardComponent { get; set; }
        public int BoardComponentId { get; set; }

        public TileComponent() { }

    }
}