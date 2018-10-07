using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IShopItemService : IMappedRepository<ShopItemDto>
    {
        IEnumerable<ShopItemDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopItemDto>> GetByShopIdAsync(long shopId);
    }
}