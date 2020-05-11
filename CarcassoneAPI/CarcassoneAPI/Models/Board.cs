using System.Collections.Generic;

namespace CarcassoneAPI.Models
{
    public class Board
    {
        public int Id { get; set; }
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        public virtual ICollection<Tile> Tiles { get; set; }

        public Board()
        {
                
        }
    }
}