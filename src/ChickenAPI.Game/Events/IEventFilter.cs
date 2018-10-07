using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public interface IEventFilter
    {
        /// <summary>
        /// Returns true
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        bool Filter(IEntity entity, ChickenEventArgs e);
    }
}