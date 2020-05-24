namespace CarcassoneAPI.Models
{
    public enum TilePosition
    {
        Center,

        TopLeft,
        Top,
        TopRight,

        RightTop,
        Right,
        RightBottom,

        BottomRight,
        Bottom,
        BottomLeft,

        LeftBottom,
        Left,
        LeftTop
    }

    public static class TilePositionExtensions
    {
        private static TilePosition Rotate(this TilePosition position)
        {
            if (position == TilePosition.Center) return TilePosition.Center;

            return (TilePosition) (((int)position + 3) % 12);
        }

        public static TilePosition Rotate(this TilePosition position, int rotation)
        {
            rotation %= 4;

            for (int i = 0; i < rotation; i++)
            {
                position = position.Rotate();
            }

            return position;
        }

    }
}
