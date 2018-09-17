using System;
using ChickenAPI.Core.Utils;

namespace ChickenAPI.Game.Features.Movement.Extensions
{
    public static class MoveChecksExtensions
    {
        private static double Octile(int x, int y)
        {
            int min = Math.Min(x, y);
            int max = Math.Max(x, y);
            return min * Math.Sqrt(2) + max - min;
        }

        private static int GetDistance(Position<short> src, short x, short y) => (int)Octile(Math.Abs(src.X - x), Math.Abs(src.Y - y));

        private static int GetDistance(Position<short> src, Position<short> dest) => (int)Octile(Math.Abs(src.X - dest.X), Math.Abs(src.Y - dest.Y));

        public static bool CanMove(this MovableComponent mov, short x, short y)
        {
            if (mov.Speed == 0)
            {
                return false;
            }

            double waitingtime = GetDistance(mov.Actual, x, y) / (double)mov.Speed;
            return mov.LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }

        public static bool CanMove(this MovableComponent mov, Position<short> newPos)
        {
            if (mov.Speed == 0)
            {
                return false;
            }

            double waitingtime = GetDistance(mov.Actual, newPos) / (double)mov.Speed;
            return mov.LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }
    }
}