using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game._ECS.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected ComponentBase(IEntity entity) => Entity = entity;

        public IEntity Entity { get; }
    }
}