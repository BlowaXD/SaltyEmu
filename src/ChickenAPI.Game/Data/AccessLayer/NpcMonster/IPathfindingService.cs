using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Data.AccessLayer.NpcMonster
{
    public interface IPathfindingService
    {
        Queue<Position<short>> GetPath(Position<short> origin, Position<short> dest, IMap map);
    }
}