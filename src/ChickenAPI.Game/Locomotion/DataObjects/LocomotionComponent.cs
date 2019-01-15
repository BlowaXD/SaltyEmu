using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Locomotion.DataObjects
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class LocomotionComponent : IComponent
    {
        public LocomotionComponent(IPlayerEntity entity) => Entity = entity;

        public bool IsVehicled { get; set; }

        public byte Speed { get; set; }

        public IEntity Entity { get; set; }
    }
}