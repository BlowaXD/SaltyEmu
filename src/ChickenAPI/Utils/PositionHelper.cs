using System;

namespace ChickenAPI.Utils
{
    public static class PositionHelper
    {
        public static int GetDistance(Position<short> current, Position<short> target) => Math.Abs(current.X - target.X) + Math.Abs(current.Y - target.Y);
    }
}