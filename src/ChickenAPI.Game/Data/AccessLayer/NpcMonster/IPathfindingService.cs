using System.Collections.Generic;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Maps;

namespace ChickenAPI.Game.Data.AccessLayer.NpcMonster
{
    public interface IPathfindingService
    {
        Queue<Position<short>> GetPath(Position<short> origin, Position<short> dest, IMap map);
    }
}