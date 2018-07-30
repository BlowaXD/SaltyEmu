using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.Game.Components
{
    public class NameComponent : IComponent
    {
        public NameComponent(IEntity entity) => Entity = entity;
        public string Name { get; set; }
        public IEntity Entity { get; }
    }
}