using ChickenAPI.Game.Features.Movement;
using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IMovableEntity
    {
        MovableComponent Movable { get; }
    }
}