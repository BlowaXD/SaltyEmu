namespace ChickenAPI.Data.Item
{
    public interface IItemInstanceDtoFactory
    {
        ItemInstanceDto CreateItem(ItemDto item, short quantity);

        ItemInstanceDto CreateItem(ItemDto item, short quantity, sbyte rarity);

        ItemInstanceDto CreateItem(ItemDto item, short quantity, sbyte rarity, byte upgrade);

        /// <summary>
        ///     Creates an item
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        ItemInstanceDto CreateItem(long itemId, short quantity);

        ItemInstanceDto CreateItem(long itemId, short quantity, sbyte rarity);

        ItemInstanceDto CreateItem(long itemId, short quantity, sbyte rarity, byte upgrade);

        /// <summary>
        ///     Generates a new item with the exact same stats as the original item
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        ItemInstanceDto Duplicate(ItemInstanceDto original);
    }
}