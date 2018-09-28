using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected ComponentBase(IEntity entity) => Entity = entity;

        public IEntity Entity { get; }
    }
}