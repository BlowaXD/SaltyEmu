using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IShopItemService : IMappedRepository<ShopItemDto>
    {
        IEnumerable<ShopItemDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopItemDto>> GetByShopIdAsync(long shopId);
    }
}