using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Shop;

namespace SaltyEmu.DatabasePlugin.Services.Shop
{
    public class RecipeDao : MappedRepositoryBase<RecipeDto, RecipeModel>, IRecipeService
    {
        public RecipeDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<RecipeDto> GetByShopId(long shopId)
        {
            try
            {
                return DbSet.Where(s => s.ShopId == shopId).ToList().Select(Mapper.Map<RecipeDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                return null;
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetByShopIdAsync(long shopId)
        {
            try
            {
                return (await DbSet.Where(s => s.ShopId == shopId).ToListAsync()).Select(Mapper.Map<RecipeDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                return null;
            }
        }
    }
}