using System.Collections.Generic;

namespace CarcassoneAPI.Models
{
    /// <summary>
    /// List of connected terrains of tile type
    /// </summary>
    public class TileTypeComponent
    {
        public int TileTypeComponentId { get; set; }
        public TerrainType TerrainType { get; set; }

        public virtual ICollection<TileTypeTerrain> Terrains { get; set; }

        public TileType TileType { get; set; }
        public int TileTypeId { get; set; }

        //public virtual ICollection<TileComponent> TileComponent { get; set; }

    }
}