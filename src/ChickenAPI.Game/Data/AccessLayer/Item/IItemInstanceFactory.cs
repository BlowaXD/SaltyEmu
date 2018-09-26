using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Data.AccessLayer.Item
{
    public interface IItemInstanceFactory
    {
        ItemInstanceDto CreateItem(ItemDto item, short quantity);
        ItemInstanceDto CreateItem(ItemDto item, short quantity, byte rarity);
        ItemInstanceDto CreateItem(ItemDto item, short quantity, byte rarity, byte upgrade);

        /// <summary>
        ///     Creates an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        ItemInstanceDto CreateItem(long itemId, short quantity);

        ItemInstanceDto CreateItem(long itemId, short quantity, byte rarity);
        ItemInstanceDto CreateItem(long itemId, short quantity, byte rarity, byte upgrade);

        /// <summary>
        ///     Generates a new item with the exact same stats as the original item
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        ItemInstanceDto Duplicate(ItemInstanceDto original);
    }
}