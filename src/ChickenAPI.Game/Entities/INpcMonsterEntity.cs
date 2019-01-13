using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Entities
{
    public interface INpcMonsterEntity : IEntity
    {
        NpcMonsterDto NpcMonster { get; }
    }
}