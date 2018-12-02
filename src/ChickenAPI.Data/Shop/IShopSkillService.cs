using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Shop
{
    public interface IShopSkillService : IMappedRepository<ShopSkillDto>
    {
        IEnumerable<ShopSkillDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopSkillDto>> GetByShopIdAsync(long shopId);
    }
}