using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Maps
{
    public interface IPathfinder
    {
        Position<short>[] FindPath(Position<short> start, Position<short> end, IMap map);
    }
}