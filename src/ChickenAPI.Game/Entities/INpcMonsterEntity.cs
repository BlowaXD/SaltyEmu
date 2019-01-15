using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Entities
{
    public interface INpcMonsterEntity : IEntity
    {
        NpcMonsterDto NpcMonster { get; }
    }
}