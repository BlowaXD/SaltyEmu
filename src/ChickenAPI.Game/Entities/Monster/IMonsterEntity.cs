using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IBattleEntity, INpcMonsterEntity, IMapMonsterEntity
    {
    }
}