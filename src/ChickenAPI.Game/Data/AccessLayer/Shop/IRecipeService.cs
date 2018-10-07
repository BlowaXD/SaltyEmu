using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IRecipeService : IMappedRepository<RecipeDto>
    {
        IEnumerable<RecipeDto> GetByShopId(long shopId);

        Task<IEnumerable<RecipeDto>> GetByShopIdAsync(long shopId);
    }
}