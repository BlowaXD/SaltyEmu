using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Game.Entities;

namespace ChickenAPI.Game.Entities.Monster
{
    public interface IMonsterEntity : IEntity, IMovableEntity, IBattleEntity, INpcMonsterEntity, IMapMonsterEntity
    {
        
    }
}