using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IShopSkillService : IMappedRepository<ShopSkillDto>
    {
        IEnumerable<ShopSkillDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopSkillDto>> GetByShopIdAsync(long shopId);
    }
}