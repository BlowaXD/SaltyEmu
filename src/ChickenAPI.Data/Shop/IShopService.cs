using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Shop
{
    public interface IShopService : IMappedRepository<ShopDto>
    {
        IEnumerable<ShopDto> GetByMapNpcId(long mapId);

        Task<IEnumerable<ShopDto>> GetByMapNpcIdAsync(long mapId);

        IEnumerable<ShopDto> GetByMapNpcIds(IEnumerable<long> npcIds);

        Task<IEnumerable<ShopDto>> GetByMapNpcIdsAsync(IEnumerable<long> npcIds);
    }
}