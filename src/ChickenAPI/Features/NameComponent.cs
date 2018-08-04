using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Game.Components
{
    public class NameComponent : IComponent
    {
        public NameComponent(IEntity entity) => Entity = entity;
        public string Name { get; set; }
        public IEntity Entity { get; }
    }
}