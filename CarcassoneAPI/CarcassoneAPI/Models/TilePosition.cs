using System;

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

            position += 3;

            if ((int)position > 12)
            {
                position = (TilePosition) ((int)position % 12);
            }

            return position;
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

        public static TilePosition GetOpposite(this TilePosition position)
        {
            if (position == TilePosition.Center) return TilePosition.Center;

            return (TilePosition)(((int)position + 6) % 12);
        }

        public static TilePosition GetPositionLeftOfMiddle(this TilePosition position)
        {
            if (position != TilePosition.Top && position != TilePosition.Right && position != TilePosition.Bottom && position != TilePosition.Left)
            {
                throw new Exception("Cannot find left/right position. You must pass one of middle terrains");
            }

            return (TilePosition)((int)position - 1);
        }

        public static TilePosition GetPositionRightOfMiddle(this TilePosition position)
        {
            if (position != TilePosition.Top && position != TilePosition.Right && position != TilePosition.Bottom && position != TilePosition.Left)
            {
                throw new Exception("Cannot find left/right position. You must pass one of middle terrains");
            }

            return (TilePosition)((int)position + 1);
        }

    }
}
