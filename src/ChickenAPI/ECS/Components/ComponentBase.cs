using ChickenAPI.ECS.Entities;

namespace ChickenAPI.ECS.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected readonly IEntity Owner;

        protected ComponentBase(IEntity entity) => Owner = entity;

        public IEntity Entity => Owner;
    }
}