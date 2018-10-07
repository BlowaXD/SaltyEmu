using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.Shop;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Shop;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Shop
{
    public class ShopDao : MappedRepositoryBase<ShopDto, ShopModel>, IShopService
    {
        public ShopDao(SaltyDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<ShopDto> GetByMapNpcId(long mapNpcId)
        {
            try
            {
                return DbSet.Where(s => s.MapNpcId == mapNpcId).ToArray().Select(Mapper.Map<ShopDto>);
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
                return (await DbSet.Where(s => s.MapNpcId == mapNpcId).ToArrayAsync()).Select(Mapper.Map<ShopDto>);
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
                return DbSet.Where(s => npcIds.Contains(s.MapNpcId)).ToArray().Select(s => Mapper.Map<ShopDto>(s));
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
                return (await DbSet.Where(s => npcIds.Contains(s.MapNpcId)).ToArrayAsync()).Select(Mapper.Map<ShopDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_NPC_IDS]", e);
                throw;
            }
        }
    }
}