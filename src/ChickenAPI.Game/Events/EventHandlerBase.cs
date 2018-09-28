using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public abstract class EventHandlerBase : IEventHandler
    {
        public abstract void Execute(IEntity entity, ChickenEventArgs args);
    }
}