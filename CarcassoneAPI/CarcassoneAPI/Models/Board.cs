using System.Collections.Generic;

namespace CarcassoneAPI.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        // stack of tiles
        public virtual ICollection<Tile> Tiles { get; set; }

    }
}