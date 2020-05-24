using System.Collections.Generic;
using System.Linq;

namespace CarcassoneAPI.Models
{
    public class TileType
    {
        public int TileTypeId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<TileTypeTerrain> Terrains { get; set; }
        public virtual ICollection<TileTypeComponent> Components { get; set; }

        public int Count { get; set; }

        public TileType()  { }

        public TileType(string name, string imgUrl, int count,
            TerrainType center,
            TerrainType topLeft, TerrainType top, TerrainType topRight,
            TerrainType rightTop, TerrainType right, TerrainType rightBottom,
            TerrainType bottomRight, TerrainType bottom, TerrainType bottomLeft,
            TerrainType leftBottom, TerrainType left, TerrainType leftTop,
            params List<TilePosition>[] components)
        {
            Name = name;
            ImageUrl = imgUrl;
            Count = count;

            Terrains = new List<TileTypeTerrain> {
                new TileTypeTerrain { Position = TilePosition.Center, TerrainType = center },

                new TileTypeTerrain { Position = TilePosition.TopLeft, TerrainType = topLeft },
                new TileTypeTerrain { Position = TilePosition.Top, TerrainType = top },
                new TileTypeTerrain { Position = TilePosition.TopRight, TerrainType = topRight },

                new TileTypeTerrain { Position = TilePosition.RightTop, TerrainType = rightTop },
                new TileTypeTerrain { Position = TilePosition.Right, TerrainType = right },
                new TileTypeTerrain { Position = TilePosition.RightBottom, TerrainType = rightBottom },

                new TileTypeTerrain { Position = TilePosition.BottomRight, TerrainType = bottomRight },
                new TileTypeTerrain { Position = TilePosition.Bottom, TerrainType = bottom },
                new TileTypeTerrain { Position = TilePosition.BottomLeft, TerrainType = bottomLeft },

                new TileTypeTerrain { Position = TilePosition.LeftBottom, TerrainType = leftBottom },
                new TileTypeTerrain { Position = TilePosition.Left, TerrainType = left },
                new TileTypeTerrain { Position = TilePosition.LeftTop, TerrainType = leftTop }
            };

            Components = new List<TileTypeComponent>();

            foreach (var positions in components)
            {
                var terrains = Terrains.Where(t => positions.Contains(t.Position)).ToList();

                Components.Add(new TileTypeComponent
                {
                    TerrainType = terrains.FirstOrDefault().TerrainType,
                    Terrains = terrains
                });
            }
        }
    }

    public static class TileTypeExtensions
    {
        public static TerrainType Top(this TileType type)
        {
            if (type == null) return TerrainType.Void;
            return type.Terrains.FirstOrDefault(terrain => terrain.Position == TilePosition.Top).TerrainType;
        }

        public static TerrainType Left(this TileType type)
        {
            if (type == null) return TerrainType.Void;
            return type.Terrains.FirstOrDefault(terrain => terrain.Position == TilePosition.Left).TerrainType;
        }

        public static TerrainType Right(this TileType type)
        {
            if (type == null) return TerrainType.Void;
            return type.Terrains.FirstOrDefault(terrain => terrain.Position == TilePosition.Right).TerrainType;
        }

        public static TerrainType Bottom(this TileType type)
        {
            if (type == null) return TerrainType.Void;

            var xxx = type.Terrains.FirstOrDefault(terrain => terrain.Position == TilePosition.Bottom);
            return xxx.TerrainType;
        }

    }



}
