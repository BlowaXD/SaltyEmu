using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.Events
{
    public abstract class EventHandlerBase : IEventHandler
    {
        public abstract void Execute(IEntity entity, ChickenEventArgs args);
    }
}