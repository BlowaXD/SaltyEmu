using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.Game.Components
{
    public class FamilyComponent : IComponent
    {
        public FamilyComponent(IEntity entity) => Entity = entity;

        public long FamilyId { get; set; }
        public byte FamilyLevel { get; set; }
        public short Players { get; set; }
        public string FamilyName { get; set; }
        public IEntity Entity { get; }
    }
}