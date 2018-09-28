using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.ECS.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected ComponentBase(IEntity entity) => Entity = entity;

        public IEntity Entity { get; }
    }
}