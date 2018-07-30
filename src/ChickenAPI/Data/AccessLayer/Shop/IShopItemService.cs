using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Shop;

namespace ChickenAPI.Data.AccessLayer.Shop
{
    public interface IShopItemService : IMappedRepository<ShopItemDto>
    {
        IEnumerable<ShopItemDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopItemDto>> GetByShopIdAsync(long shopId);
    }
}