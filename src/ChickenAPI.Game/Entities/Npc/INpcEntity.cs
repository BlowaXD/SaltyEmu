using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Shops;

namespace ChickenAPI.Game.Entities.Npc
{
    public interface INpcEntity : IBattleEntity, IMapNpcEntity, IShopEntity, INpcMonsterEntity
    {
        /// <summary>
        /// Shop access
        /// </summary>
        Shop Shop { get; }
    }
}