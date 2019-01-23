using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities;

namespace ChickenAPI.Game.IAs
{
    public interface IAiEntity : IBattleEntity, INpcMonsterEntity
    {
        Position<short>[] Waypoints { get; set; }
    }
}