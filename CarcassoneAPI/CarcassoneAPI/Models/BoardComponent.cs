using System.Collections.Generic;

namespace CarcassoneAPI.Models
{
    public class BoardComponent
    {
        public int BoardComponentId { get; set; }
        public TerrainType TerrainType { get; set; }
        public bool IsOpen { get; set; } // computed from all sub-components (jen get???)
        public int Points { get; set; }

        public Board Board { get; set; }
        public int BoardId { get; set; }

        public virtual ICollection<TileComponent> Components { get; set; }
    }
}
