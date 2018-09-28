using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Visibility
{
    public interface IVisibleEntity : IEntity, IVisibleCapacity
    {
        /// <summary>
        /// Triggered event when the entity becomes invisible
        /// </summary>
        event EventHandlerWithoutArgs<IVisibleEntity> Invisible;

        /// <summary>
        /// Triggered event when the entity becomes visible
        /// </summary>
        event EventHandlerWithoutArgs<IVisibleEntity> Visible;

    }
}