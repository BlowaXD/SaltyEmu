using ChickenAPI.Data.TransferObjects.NpcMonster;

namespace ChickenAPI.Game.Game.Entities
{
    public interface INpcMonsterEntity
    {
        NpcMonsterDto NpcMonster { get; }
    }
}