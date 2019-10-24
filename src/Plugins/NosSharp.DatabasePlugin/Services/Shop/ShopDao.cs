using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Shop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Shop;

namespace SaltyEmu.DatabasePlugin.Services.Shop
{
    public class ShopDao : MappedRepositoryBase<ShopDto, ShopModel>, IShopService
    {
        private readonly Dictionary<long, ShopDto[]> _shops;

        public ShopDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
        {
            IEnumerable<ShopDto> all = Get();
            IEnumerable<IGrouping<long, ShopDto>> tmp = all.GroupBy(s => s.MapNpcId);
            _shops = new Dictionary<long, ShopDto[]>(tmp.ToDictionary(s => s.Key, dtos => dtos.ToArray()));
        }

        public IEnumerable<ShopDto> GetByMapNpcId(long mapNpcId)
        {
            try
            {
                if (!_shops.TryGetValue(mapNpcId, out ShopDto[] shops))
                {
                    shops = DbSet.Where(s => s.MapNpcId == mapNpcId).ToArray().Select(Mapper.Map<ShopDto>).ToArray();
                    _shops[mapNpcId] = shops;
                }

                return shops;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_NPC_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<ShopDto>> GetByMapNpcIdAsync(long mapNpcId)
        {
            try
            {
                if (!_shops.TryGetValue(mapNpcId, out ShopDto[] shops))
                {
                    shops = (await DbSet.Where(s => s.MapNpcId == mapNpcId).ToArrayAsync()).Select(Mapper.Map<ShopDto>).ToArray();
                    _shops[mapNpcId] = shops;
                }

                return shops;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_NPC_ID]", e);
                throw;
            }
        }

        public IEnumerable<ShopDto> GetByMapNpcIds(IEnumerable<long> npcIds)
        {
            try
            {
                List<ShopDto> tmp = new List<ShopDto>();
                foreach (long mapNpcId in npcIds)
                {
                    if (!_shops.TryGetValue(mapNpcId, out ShopDto[] shops))
                    {
                        shops = DbSet.Where(s => s.MapNpcId == mapNpcId).ToArray().Select(Mapper.Map<ShopDto>).ToArray();
                        _shops[mapNpcId] = shops;
                    }

                    tmp.AddRange(shops);
                }

                return tmp;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_NPC_IDS]", e);
                throw;
            }
        }

        public async Task<IEnumerable<ShopDto>> GetByMapNpcIdsAsync(IEnumerable<long> npcIds)
        {
            try
            {
                List<ShopDto> tmp = new List<ShopDto>();
                foreach (long mapNpcId in npcIds)
                {
                    if (!_shops.TryGetValue(mapNpcId, out ShopDto[] shops))
                    {
                        shops = (await DbSet.Where(s => s.MapNpcId == mapNpcId).ToArrayAsync()).Select(Mapper.Map<ShopDto>).ToArray();
                        _shops[mapNpcId] = shops;
                    }

                    tmp.AddRange(shops);
                }

                return tmp;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_NPC_IDS]", e);
                throw;
            }
        }
    }
}