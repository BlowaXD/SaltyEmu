using ChickenAPI.Game.Features.Movement;

namespace ChickenAPI.Game.Entities
{
    public interface IMovableEntity
    {
        MovableComponent Movable { get; }
    }
}