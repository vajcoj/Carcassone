namespace CarcassoneAPI.Models
{
    public class Tile
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }

        public Board Board { get; set; }
        public int BoardId { get; set; }

        public Tile()
        {

        }

    }
}