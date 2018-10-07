using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IRecipeItemService : IMappedRepository<RecipeItemDto>
    {
        IEnumerable<RecipeItemDto> GetByRecipeId(long recipeId);

        Task<IEnumerable<RecipeItemDto>> GetByRecipeIdAsync(long recipeId);
    }
}