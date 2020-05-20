namespace CarcassoneAPI.Models
{
    public enum TilePosition
    {
        TOP_LEFT,
        TOP,
        TOP_RIGHT,

        RIGHT_TOP,
        RIGHT,
        RIGHT_BOTTOM,

        BOTTOM_RIGHT,
        BOTTOM,
        BOTTOM_LEFT,

        LEFT_BOTTOM,
        LEFT,
        LEFT_TOP,

        CENTER 
    }

    // top, up, left, right - one of - pro zjisteni, jestli je napojena

    // extension metoda na rotate

    public static class TilePositionExtensions
    {
        public static TilePosition Rotate(this TilePosition position, int rotation)
        {



            rotation = rotation % 4;

            // for

            return TilePosition.BOTTOM;
        }

    }
}
