using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Shop;

namespace ChickenAPI.Data.AccessLayer.Shop
{
    public interface IShopService : IMappedRepository<ShopDto>
    {
        IEnumerable<ShopDto> GetByMapNpcId(long mapId);

        Task<IEnumerable<ShopDto>> GetByMapNpcIdAsync(long mapId);
    }
}