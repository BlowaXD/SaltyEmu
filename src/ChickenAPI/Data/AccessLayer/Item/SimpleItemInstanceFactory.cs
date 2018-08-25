using System;
using ChickenAPI.Game.Data.TransferObjects.Item;

namespace ChickenAPI.Game.Data.AccessLayer.Item
{
    public class SimpleItemInstanceFactory : IItemInstanceFactory
    {
        private readonly IItemService _itemService;

        public SimpleItemInstanceFactory(IItemService itemService) => _itemService = itemService;


        public ItemInstanceDto CreateItem(ItemDto item, short quantity) => CreateItem(item, quantity, 0, 0);

        public ItemInstanceDto CreateItem(ItemDto item, short quantity, byte rarity) => CreateItem(item, quantity, rarity, 0);

        public ItemInstanceDto CreateItem(ItemDto item, short quantity, byte rarity, byte upgrade) => new ItemInstanceDto
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
        public ItemInstanceDto CreateItem(long itemId, short quantity, byte rarity) => CreateItem(_itemService.GetById(itemId), quantity, rarity, 0);

        public ItemInstanceDto CreateItem(long itemId, short quantity, byte rarity, byte upgrade) => CreateItem(_itemService.GetById(itemId), quantity, rarity, upgrade);

        public ItemInstanceDto Duplicate(ItemInstanceDto original) => original.Clone() as ItemInstanceDto;
    }
}