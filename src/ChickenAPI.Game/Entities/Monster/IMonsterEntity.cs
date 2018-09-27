using ChickenAPI.Game.Battle;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IBattleEntity, INpcMonsterEntity, IMapMonsterEntity
    {
    }
}