using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.IAs;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IAiEntity, INpcMonsterEntity, IMapMonsterEntity
    {
    }
}