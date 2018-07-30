using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Shop;

namespace ChickenAPI.Data.AccessLayer.Shop
{
    public interface IRecipeService : IMappedRepository<RecipeDto>
    {
        IEnumerable<RecipeDto> GetByShopId(long shopId);

        Task<IEnumerable<RecipeDto>> GetByShopIdAsync(long shopId);
    }
}