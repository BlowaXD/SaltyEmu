using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IRecipeItemService : IMappedRepository<RecipeItemDto>
    {
        IEnumerable<RecipeItemDto> GetByRecipeId(long recipeId);

        Task<IEnumerable<RecipeItemDto>> GetByRecipeIdAsync(long recipeId);
    }
}