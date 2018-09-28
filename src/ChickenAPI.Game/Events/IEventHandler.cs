using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.Events
{
    public interface IEventHandler
    {
        void Execute(IEntity entity, ChickenEventArgs args);
    }
}