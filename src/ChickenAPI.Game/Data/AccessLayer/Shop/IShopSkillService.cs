using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Data.AccessLayer.Shop
{
    public interface IShopSkillService : IMappedRepository<ShopSkillDto>
    {
        IEnumerable<ShopSkillDto> GetByShopId(long shopId);

        Task<IEnumerable<ShopSkillDto>> GetByShopIdAsync(long shopId);
    }
}