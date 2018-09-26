using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IRecipeService : IMappedRepository<RecipeDto>
    {
        IEnumerable<RecipeDto> GetByShopId(long shopId);

        Task<IEnumerable<RecipeDto>> GetByShopIdAsync(long shopId);
    }
}