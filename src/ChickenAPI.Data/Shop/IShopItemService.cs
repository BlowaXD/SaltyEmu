using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Shop
{
    public interface IShopItemService : IMappedRepository<ShopItemDto>
    {
        IEnumerable<ShopItemDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopItemDto>> GetByShopIdAsync(long shopId);
    }
}