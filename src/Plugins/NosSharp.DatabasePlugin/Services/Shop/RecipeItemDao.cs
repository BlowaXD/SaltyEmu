using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Shop;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Shop
{
    public class RecipeItemDao : MappedRepositoryBase<RecipeItemDto, RecipeItemModel>, IRecipeItemService
    {
        public RecipeItemDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<RecipeItemDto> GetByRecipeId(long recipeId)
        {
            try
            {
                return DbSet.Where(s => s.RecipeId == recipeId).ToList().Select(Mapper.Map<RecipeItemDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_RECIPE_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<RecipeItemDto>> GetByRecipeIdAsync(long recipeId)
        {
            try
            {
                return (await DbSet.Where(s => s.RecipeId == recipeId).ToListAsync()).Select(Mapper.Map<RecipeItemDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_RECIPE_ID]", e);
                throw;
            }
        }
    }
}