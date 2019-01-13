using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.IAs
{
    public interface IAiEntity : IBattleEntity
    {
        Position<short>[] Waypoints { get; set; }
    }
}