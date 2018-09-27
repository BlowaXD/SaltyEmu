using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Battle;
using ChickenAPI.Game.Battle.Interfaces;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IEntity, IBattleEntity, INpcMonsterEntity, IMapMonsterEntity
    {
    }
}