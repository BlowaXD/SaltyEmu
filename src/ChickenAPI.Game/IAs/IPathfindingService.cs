using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.IAs
{
    public interface IPathfindingService
    {
        Queue<Position<short>> GetPath(Position<short> origin, Position<short> dest, IMap map);
    }
}