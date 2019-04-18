using System;

namespace ChickenAPI.Core.Utils
{
    public static class PositionHelper
    {
        public static double Octile(int x, int y)
        {
            int min = Math.Min(x, y);
            int max = Math.Max(x, y);
            return min * Math.Sqrt(2) + max - min;
        }

        public static int GetDistance(Position<short> src, short x, short y) => (int)Octile(Math.Abs(src.X - x), Math.Abs(src.Y - y));

        public static int GetDistance(Position<short> src, Position<short> dest) => (int)Octile(Math.Abs(src.X - dest.X), Math.Abs(src.Y - dest.Y));
    }
}