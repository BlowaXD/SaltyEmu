using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.Shop;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Shop
{
    public class ShopItemDao : MappedRepositoryBase<ShopItemDto, ShopItemModel>, IShopItemService
    {
        public ShopItemDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<ShopItemDto> GetByShopId(long shopId)
        {
            try
            {
                return DbSet.Where(s => s.ShopId == shopId).ToList().Select(Mapper.Map<ShopItemDto>);
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
