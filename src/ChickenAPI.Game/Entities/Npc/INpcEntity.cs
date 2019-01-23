using ChickenAPI.Game.IAs;
using ChickenAPI.Game.Shops;

namespace ChickenAPI.Game.Entities.Npc
{
    public interface INpcEntity : IAiEntity, IMapNpcEntity, IShopEntity, INpcMonsterEntity
    {
        /// <summary>
        ///     Shop access
        /// </summary>
        Shop Shop { get; }
    }
}