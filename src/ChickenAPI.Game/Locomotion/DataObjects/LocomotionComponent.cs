using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Locomotion.DataObjects
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class LocomotionComponent : IComponent
    {
        public LocomotionComponent(IPlayerEntity entity)
        {
            Entity = entity;
        }

        public bool IsVehicled { get; set; }

        public IEntity Entity { get; set; }

        public byte Speed { get; set; }
    }
}