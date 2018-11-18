﻿using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.Data.AccessLayer.Shop;

namespace ChickenAPI.Game.Shops
{
    public class Shop : ShopDto
    {
        private static readonly IShopItemService _shopItemService = new Lazy<IShopItemService>(() => ChickenContainer.Instance.Resolve<IShopItemService>()).Value;
        private static readonly IShopSkillService _shopSkillService = new Lazy<IShopSkillService>(() => ChickenContainer.Instance.Resolve<IShopSkillService>()).Value;
        public readonly HashSet<ShopItemDto> Items;
        public readonly HashSet<ShopSkillDto> Skills;

        public Shop(ShopDto shop)
        {
            Id = shop.Id;
            MapNpcId = shop.MapNpcId;
            Name = shop.Name;
            MenuType = shop.MenuType;
            ShopType = shop.ShopType;

            Items = new HashSet<ShopItemDto>(_shopItemService.GetByShopId(Id));
            Skills = new HashSet<ShopSkillDto>(_shopSkillService.GetByShopId(Id));
        }
    }
}