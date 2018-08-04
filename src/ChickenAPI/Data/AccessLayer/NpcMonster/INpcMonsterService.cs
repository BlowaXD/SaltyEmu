using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.NpcMonster;

namespace ChickenAPI.Game.Data.AccessLayer.NpcMonster
{
    public interface INpcMonsterService : IMappedRepository<NpcMonsterDto>
    {
    }
}