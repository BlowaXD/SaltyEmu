using ChickenAPI.Game.Game.Components;

namespace ChickenAPI.Game.Entities
{
    public interface IMovableEntity
    {
        MovableComponent Movable { get; }
    }
}