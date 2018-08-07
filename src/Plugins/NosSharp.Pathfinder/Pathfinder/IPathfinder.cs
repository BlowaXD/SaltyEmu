using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Maps;

namespace NosSharp.Pathfinder
{
    public interface IPathfinder
    {
        Position<short>[] FindPath(Position<short> start, Position<short> end, IMap map);
    }
}