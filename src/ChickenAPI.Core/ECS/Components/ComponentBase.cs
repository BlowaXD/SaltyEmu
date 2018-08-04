using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Core.ECS.Components
{
    public abstract class ComponentBase : IComponent
    {
        protected readonly IEntity Owner;

        protected ComponentBase(IEntity entity) => Owner = entity;

        public IEntity Entity => Owner;
    }
}