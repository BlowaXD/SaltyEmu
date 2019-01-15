using ChickenAPI.Game.Locomotion.DataObjects;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Locomotion
{
    public interface ILocomotionEntity : IEntity, ILocomotionCapacity
    {
        LocomotionComponent Locomotion { get; }
    }
}