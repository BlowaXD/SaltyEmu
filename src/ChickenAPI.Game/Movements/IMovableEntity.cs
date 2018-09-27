using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Features.Movement;

namespace ChickenAPI.Game.Movements
{
    public interface IMovableEntity : IEntity
    {
        MovableComponent Movable { get; }
    }
}