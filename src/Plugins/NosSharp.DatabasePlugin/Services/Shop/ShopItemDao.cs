using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Shop;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Shop
{
    public class ShopItemDao : MappedRepositoryBase<ShopItemDto, ShopItemModel>, IShopItemService
    {
        private readonly Dictionary<long, ShopItemDto[]> _shops;

        public ShopItemDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
            _shops = new Dictionary<long, ShopItemDto[]>(Get().GroupBy(s => s.ShopId).ToDictionary(s => s.Key, s => s.ToArray()));
        }

        public IEnumerable<ShopItemDto> GetByShopId(long shopId)
        {
            try
            {
                if (!_shops.TryGetValue(shopId, out ShopItemDto[] items))
                {
                    items = DbSet.Where(s => s.ShopId == shopId).ToList().Select(Mapper.Map<ShopItemDto>).ToArray();
                    _shops[shopId] = items;
                }

                return items;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<ShopItemDto>> GetByShopIdAsync(long shopId)
        {
            try
            {
                return (await DbSet.Where(s => s.ShopId == shopId).ToListAsync()).Select(Mapper.Map<ShopItemDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                throw;
            }
        }
    }
}