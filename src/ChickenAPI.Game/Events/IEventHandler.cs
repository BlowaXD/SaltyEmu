using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Events
{
    public interface IEventHandler
    {
        void Execute(IEntity entity, ChickenEventArgs e);
    }
}