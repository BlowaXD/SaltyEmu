using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Visibility;

namespace ChickenAPI.Game.Movements
{
    public interface IMovableCapacity : IVisibleCapacity
    {
        /// <summary>
        /// Checks if the object can move
        /// </summary>
        bool CanMove { get; }

        Position<short> Actual { get; }
        Position<short> Destination { get; }

        /// <summary>
        /// Sets the position of the entity
        /// </summary>
        /// <param name="position"></param>
        void SetPosition(Position<short> position);

        /// <summary>
        /// Sets the position of the entity
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void SetPosition(short x, short y);
    }
}