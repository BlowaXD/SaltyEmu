using System;
using Autofac;
using ChickenAPI.Data.TransferObjects.Item;
using ChickenAPI.Utils;

namespace ChickenAPI.Data.AccessLayer.Item
{
    public class SimpleItemInstanceFactory : IItemInstanceFactory
    {
        private readonly IItemService _itemService;

        public SimpleItemInstanceFactory(IItemService itemService)
        {
            _itemService = itemService;
        }


        public ItemInstanceDto CreateItem(ItemDto item, short quantity) => new ItemInstanceDto
        {
            Id = Guid.NewGuid(),
            Item = item,
            ItemId = item.Id,
            Amount = quantity,
            Type = item.Type
        };

        public ItemInstanceDto CreateItem(long itemId, short quantity) => CreateItem(_itemService.GetById(itemId), quantity);
    }
}