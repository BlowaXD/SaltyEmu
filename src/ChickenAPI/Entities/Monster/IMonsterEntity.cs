using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IEntity, IMovableEntity, IBattleEntity, INpcMonsterEntity, IMapMonsterEntity
    {
    }
}