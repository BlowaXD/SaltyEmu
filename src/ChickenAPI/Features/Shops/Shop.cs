using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.AccessLayer.Shop;
using ChickenAPI.Data.TransferObjects.Shop;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Features.Shops
{
    public class Shop : ShopDto
    {
        public Shop(ShopDto shop)
        {
            Items = new HashSet<ShopItemDto>();
            Skills = new HashSet<ShopSkillDto>();

            Id = shop.Id;
            MapNpcId = shop.MapNpcId;
            Name = shop.Name;
            MenuType = shop.MenuType;
            ShopType = shop.ShopType;

            Items.UnionWith(Container.Instance.Resolve<IShopItemService>().GetByShopId(Id));
            Skills.UnionWith(Container.Instance.Resolve<IShopSkillService>().GetByShopId(Id));
        }

        public HashSet<ShopItemDto> Items;
        public HashSet<ShopSkillDto> Skills;
    }
}
