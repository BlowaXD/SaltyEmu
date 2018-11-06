using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Visibility;

namespace ChickenAPI.Game.Movements
{
    public interface IMovableCapacity : IVisibleCapacity
    {
        Position<short> Position { get; }

        /// <summary>
        /// Tells if the object is sitting or not
        /// </summary>
        bool IsSitting { get; }

        /// <summary>
        /// Tells if the object is walking or not
        /// </summary>
        bool IsWalking { get; }

        /// <summary>
        ///     Checks if the object can move
        /// </summary>
        bool CanMove { get; }

        /// <summary>
        /// Tells if the object is standing or not
        /// </summary>
        bool IsStanding { get; }

        /// <summary>
        /// Gives the Movement Speed of the object
        /// </summary>
        byte Speed { get; set; }

        DateTime LastMove { get; }

        Position<short> Actual { get; }
        Position<short> Destination { get; }
    }
}