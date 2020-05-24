using CarcassoneAPI.Models;
using System.Collections.Generic;

namespace CarcassoneAPI.Data
{
    public static class TileTypesSeed
    {
        public static List<TileType> GetTileTypes()
        {
            return new List<TileType>
            {
                new TileType("Castle1", "castle1", 3,
                    TerrainType.Field,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Top }, // castle
                    new List<TilePosition> { TilePosition.Bottom, TilePosition.Left, TilePosition.Right } // field
                ),

                new TileType("Castle2", "castle2", 3,
                    TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Top, TilePosition.Right }, // castle
                    new List<TilePosition> { TilePosition.Bottom, TilePosition.Left } // field
                ),

               new TileType("Castle3", "castle3", 2,
                    TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    new List<TilePosition> { TilePosition.Top, TilePosition.Right, TilePosition.Left }, // castle
                    new List<TilePosition> { TilePosition.Bottom } // field
              ),

              new TileType("Castle3_Road", "castle3road", 2,
                    TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    new List<TilePosition> { TilePosition.Top, TilePosition.Right, TilePosition.Left }, // castle
                    new List<TilePosition> { TilePosition.Bottom }, // road
                    new List<TilePosition> { TilePosition.BottomRight }, // field right
                    new List<TilePosition> { TilePosition.BottomLeft } // field left
              ),

               new TileType("Castle4", "castle4", 1,
                    TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    new List<TilePosition> { TilePosition.Top, TilePosition.Right, TilePosition.Left, TilePosition.Bottom } // castle
               ),

                new TileType("Road1", "road1", 4,
                    TerrainType.Road,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Left, TilePosition.Right }, // road L-R 
                    new List<TilePosition> { TilePosition.Top, TilePosition.LeftTop, TilePosition.RightTop }, // field above road
                    new List<TilePosition> { TilePosition.Bottom, TilePosition.LeftBottom, TilePosition.RightBottom } // field under road
                ),

                new TileType("Road2", "road2", 4,
                    TerrainType.Road,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Top, TilePosition.Right }, // road T-R 
                    new List<TilePosition> { TilePosition.TopRight, TilePosition.RightTop }, // smal field
                    new List<TilePosition> { TilePosition.TopLeft, TilePosition.Left, TilePosition.Bottom, TilePosition.RightBottom } // big field
                ),

                new TileType("Crossroad1", "crossroad1", 2,
                    TerrainType.Road,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Top }, // road T
                    new List<TilePosition> { TilePosition.Right }, // road R
                    new List<TilePosition> { TilePosition.Bottom }, // road B
                    new List<TilePosition> { TilePosition.Left }, // road L
                    new List<TilePosition> { TilePosition.TopRight, TilePosition.RightTop }, // field TR
                    new List<TilePosition> { TilePosition.RightBottom, TilePosition.BottomRight }, // field RB
                    new List<TilePosition> { TilePosition.BottomLeft, TilePosition.LeftBottom }, // field BL
                    new List<TilePosition> { TilePosition.LeftTop, TilePosition.TopLeft } // field LT
                ),

                new TileType("Crossroad2", "crossroad2", 2,
                    TerrainType.Road,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Top }, // road T
                    new List<TilePosition> { TilePosition.Right }, // road R
                    new List<TilePosition> { TilePosition.Bottom }, // road B
                    new List<TilePosition> { TilePosition.Left }, // road L
                    new List<TilePosition> { TilePosition.TopRight, TilePosition.RightTop }, // field TR
                    new List<TilePosition> { TilePosition.Bottom,  TilePosition.RightBottom, TilePosition.LeftBottom }, // field R
                    new List<TilePosition> { TilePosition.LeftTop, TilePosition.TopLeft } // field LT
                ),

                new TileType("Castle1_Road1", "castle1road1", 2,
                    TerrainType.Road,
                    TerrainType.Castle, TerrainType.Castle, TerrainType.Castle,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    TerrainType.Field, TerrainType.Field, TerrainType.Field,
                    TerrainType.Field, TerrainType.Road, TerrainType.Field,
                    new List<TilePosition> { TilePosition.Left, TilePosition.Right }, // road L-R 
                    new List<TilePosition> { TilePosition.Top }, // castle top
                    new List<TilePosition> { TilePosition.LeftTop, TilePosition.RightTop }, // field above road
                    new List<TilePosition> { TilePosition.LeftBottom, TilePosition.RightBottom, TilePosition.Bottom } // field under road
                ),


            };
        }

    }
}
