using System;
using ChickenAPI.Core.Utils;

namespace ChickenAPI.Game.Movements.Extensions
{
    public static class MoveChecksExtensions
    {
        public static bool CanMove(this IMovableEntity mov, short x, short y)
        {
            if (mov.Speed == 0)
            {
                return false;
            }

            double waitingtime = PositionHelper.GetDistance(mov.Position, x, y) / (double)mov.Speed;
            return mov.LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }

        public static bool CanMove(this IMovableEntity mov, Position<short> newPos)
        {
            if (mov.Speed == 0)
            {
                return false;
            }

            double waitingtime = PositionHelper.GetDistance(mov.Position, newPos) / (double)mov.Speed;
            return mov.LastMove.AddMilliseconds(waitingtime) <= DateTime.UtcNow;
        }

        /// <summary>
        ///     Gets the distance between two movable entities at their actual position
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static int GetDistance(this IMovableEntity src, IMovableEntity dest) => PositionHelper.GetDistance(src.Position, dest.Position);
    }
}