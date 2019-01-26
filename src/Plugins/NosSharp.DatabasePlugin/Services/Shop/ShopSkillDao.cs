using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Shop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Shop;

namespace SaltyEmu.DatabasePlugin.Services.Shop
{
    public class ShopSkillDao : MappedRepositoryBase<ShopSkillDto, ShopSkillModel>, IShopSkillService
    {
        private readonly Dictionary<long, ShopSkillDto[]> _shop;

        public ShopSkillDao(DbContext context, IMapper mapper) : base(context, mapper)
        {
            _shop = new Dictionary<long, ShopSkillDto[]>(Get().GroupBy(s => s.ShopId).ToDictionary(s => s.Key, s => s.ToArray()));
        }

        public IEnumerable<ShopSkillDto> GetByShopId(long shopId)
        {
            try
            {
                if (!_shop.TryGetValue(shopId, out ShopSkillDto[] skills))
                {
                    skills = DbSet.Where(s => s.ShopId == shopId).ToList().Select(Mapper.Map<ShopSkillDto>).ToArray();
                    _shop[shopId] = skills;
                }

                return skills;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<ShopSkillDto>> GetByShopIdAsync(long shopId)
        {
            try
            {
                if (!_shop.TryGetValue(shopId, out ShopSkillDto[] skills))
                {
                    skills = (await DbSet.Where(s => s.ShopId == shopId).ToListAsync()).Select(Mapper.Map<ShopSkillDto>).ToArray();
                    _shop[shopId] = skills;
                }

                return skills;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_SHOP_ID]", e);
                throw;
            }
        }
    }
}