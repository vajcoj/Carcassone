using System.Collections.Generic;

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

    }
}
