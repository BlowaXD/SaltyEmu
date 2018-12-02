using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Shop
{
    public interface IRecipeService : IMappedRepository<RecipeDto>
    {
        IEnumerable<RecipeDto> GetByShopId(long shopId);

        Task<IEnumerable<RecipeDto>> GetByShopIdAsync(long shopId);
    }
}