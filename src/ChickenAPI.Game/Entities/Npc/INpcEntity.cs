using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Battle;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Movements;

namespace ChickenAPI.Game.Entities.Npc
{
    public interface INpcEntity : IEntity, IBattleEntity, IMovableEntity, IMapNpcEntity
    {
    }
}