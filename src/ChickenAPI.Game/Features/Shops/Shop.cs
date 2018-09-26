using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Features.Shops
{
    public class Shop : ShopDto
    {
        public readonly HashSet<ShopItemDto> Items;
        public readonly HashSet<ShopSkillDto> Skills;

        public Shop(ShopDto shop)
        {
            Id = shop.Id;
            MapNpcId = shop.MapNpcId;
            Name = shop.Name;
            MenuType = shop.MenuType;
            ShopType = shop.ShopType;

            Items = new HashSet<ShopItemDto>(ChickenContainer.Instance.Resolve<IShopItemService>().GetByShopId(Id));
            Skills = new HashSet<ShopSkillDto>(ChickenContainer.Instance.Resolve<IShopSkillService>().GetByShopId(Id));
        }
    }
}