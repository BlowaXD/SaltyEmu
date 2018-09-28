using ChickenAPI.Game.Battle;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IEntity, IBattleEntity, INpcMonsterEntity, IMapMonsterEntity
    {
    }
}