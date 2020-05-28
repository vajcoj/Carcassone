using System.Collections.Generic;
using System.Linq;

namespace CarcassoneAPI.Models
{
    public class Tile
    {
        public int TileId { get; set; }
        public int X { get; set; } // not null!!!
        public int Y { get; set; }
        public int Rotation { get; set; }
        public string Color { get; set; } // TODO remove

        public virtual ICollection<TileComponent> Components { get; set; }

        public Board Board { get; set; }
        public int BoardId { get; set; }

        public TileType TileType { get; set; }
        public int TileTypeId { get; set; }


    }

    public static class TileExtensions
    {
        private static TileTypeTerrain GetRotatedTerrain(this Tile tile, TilePosition position)
        {
            var terr = tile.TileType.Terrains.Where(t => t.Position.Rotate(tile.Rotation) == position).SingleOrDefault();

            return terr;
        }

        public static TerrainType GetTerrainAt(this Tile tile, TilePosition position)
        {
            var terr = tile.GetRotatedTerrain(position);

            var ret = terr != null ? terr.TerrainType : TerrainType.Void;

            return ret;
        }

        public static TileComponent GetComponentAt(this Tile tile, TilePosition position)
        {
            var terr = tile.GetRotatedTerrain(position);

            var comp = tile.Components.Where(c => c.TileTypeComponent.Terrains.Contains(terr)).SingleOrDefault();

            return comp;
        }

    }
}