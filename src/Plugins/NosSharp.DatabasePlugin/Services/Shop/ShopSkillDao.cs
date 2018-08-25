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
    public class ShopSkillDao : MappedRepositoryBase<ShopSkillDto, ShopSkillModel>, IShopSkillService
    {
        public ShopSkillDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<ShopSkillDto> GetByShopId(long shopId)
        {
            try
            {
                return DbSet.Where(s => s.ShopId == shopId).ToList().Select(Mapper.Map<ShopSkillDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<ShopSkillDto>> GetByShopIdAsync(long shopId)
        {
            try
            {
                return (await DbSet.Where(s => s.ShopId == shopId).ToListAsync()).Select(Mapper.Map<ShopSkillDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                throw;
            }
        }
    }
}