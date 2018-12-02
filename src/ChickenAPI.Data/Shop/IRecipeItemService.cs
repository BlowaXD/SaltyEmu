using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Shop
{
    public interface IRecipeItemService : IMappedRepository<RecipeItemDto>
    {
        IEnumerable<RecipeItemDto> GetByRecipeId(long recipeId);

        Task<IEnumerable<RecipeItemDto>> GetByRecipeIdAsync(long recipeId);
    }
}