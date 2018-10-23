using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Movements.DataObjects;

namespace ChickenAPI.Game.Movements.Extensions
{
    public static class MoveChecksExtensions
    {
        public static bool CanMove(this MovableComponent mov, short x, short y)
        {
            if (mov.Speed == 0)
            {
                return false;
            }

            double waitingtime = PositionHelper.GetDistance(mov.Actual, x, y) / (double)mov.Speed;
            return mov.LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }

        public static bool CanMove(this MovableComponent mov, Position<short> newPos)
        {
            if (mov.Speed == 0)
            {
                return false;
            }

            double waitingtime = PositionHelper.GetDistance(mov.Actual, newPos) / (double)mov.Speed;
            return mov.LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the distance between two movable entities at their actual position
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static int GetDistance(this MovableComponent src, MovableComponent dest)
        {
            return PositionHelper.GetDistance(src.Actual, dest.Actual);
        }

        public static int GetDistance(this IMovableEntity src, IMovableEntity dest)
        {
            return PositionHelper.GetDistance(src.Actual, dest.Actual);
        }
    }
}