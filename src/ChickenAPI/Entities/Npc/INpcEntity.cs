using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Entities.Npc
{
    public interface INpcEntity : IEntity, IBattleEntity, IMovableEntity, IMapNpcEntity
    {
    }
}