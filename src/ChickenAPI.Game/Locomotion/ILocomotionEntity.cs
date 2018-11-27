using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Locomotion.DataObjects;

namespace ChickenAPI.Game.Locomotion
{
    public interface ILocomotionEntity : IEntity, ILocomotionCapacity
    {
        LocomotionComponent Locomotion { get; }
    }
}