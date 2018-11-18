using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Features.Shops;
using ChickenAPI.Game.Shops;

namespace ChickenAPI.Game.Entities.Npc
{
    public interface INpcEntity : IBattleEntity, IMapNpcEntity, IShopCapacity
    {
    }
}