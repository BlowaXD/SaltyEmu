using System;

namespace ChickenAPI.Data.Item
{
    public class SimpleItemInstanceDtoFactory : IItemInstanceDtoFactory
    {
        private readonly IItemService _itemService;

        public SimpleItemInstanceDtoFactory(IItemService itemService) => _itemService = itemService;

        public ItemInstanceDto CreateItem(ItemDto item, short quantity) => CreateItem(item, quantity, 0, 0);

        public ItemInstanceDto CreateItem(ItemDto item, short quantity, sbyte rarity) => CreateItem(item, quantity, rarity, 0);

        public ItemInstanceDto CreateItem(ItemDto item, short quantity, sbyte rarity, byte upgrade) => new ItemInstanceDto
        {
            Id = Guid.NewGuid(),
            Item = item,
            ItemId = item.Id,
            Amount = quantity,
            Type = item.Type,
            Rarity = rarity,
            Upgrade = upgrade
        };

        public ItemInstanceDto CreateItem(long itemId, short quantity) => CreateItem(_itemService.GetById(itemId), quantity, 0, 0);

        public ItemInstanceDto CreateItem(long itemId, short quantity, sbyte rarity) => CreateItem(_itemService.GetById(itemId), quantity, rarity, 0);

        public ItemInstanceDto CreateItem(long itemId, short quantity, sbyte rarity, byte upgrade) => CreateItem(_itemService.GetById(itemId), quantity, rarity, upgrade);

        public ItemInstanceDto Duplicate(ItemInstanceDto original)
        {
            object tmp = MemberwiseClone();
            if (!(tmp is ItemInstanceDto newObject))
            {
                return null;
            }

            newObject.Id = Guid.NewGuid();
            return newObject;
        }
    }
}