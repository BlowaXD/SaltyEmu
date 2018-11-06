using System;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Visibility;

namespace ChickenAPI.Game.Movements
{
    public interface IMovableEntity : IVisibleEntity, IMovableCapacity
    {
        MovableComponent Movable { get; }
    }
}