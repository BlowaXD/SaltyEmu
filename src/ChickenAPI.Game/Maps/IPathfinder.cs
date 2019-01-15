using ChickenAPI.Core.Utils;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Maps
{
    public interface IPathfinder
    {
        Position<short>[] FindPath(Position<short> start, Position<short> end, IMap map);
        Position<short>[] GetNeighbors(Position<short> pos, IMap map);
    }
}