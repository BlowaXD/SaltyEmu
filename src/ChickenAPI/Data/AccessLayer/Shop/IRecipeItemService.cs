using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Shop;

namespace ChickenAPI.Data.AccessLayer.Shop
{
    public interface IRecipeItemService : IMappedRepository<RecipeItemDto>
    {
        IEnumerable<RecipeItemDto> GetByRecipeId(long recipeId);

        Task<IEnumerable<RecipeItemDto>> GetByRecipeIdAsync(long recipeId);
    }
}