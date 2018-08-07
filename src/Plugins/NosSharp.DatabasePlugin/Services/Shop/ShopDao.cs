using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.TransferObjects.Shop;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.NpcMonster;
using NosSharp.DatabasePlugin.Models.Shop;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Shop
{
    public class ShopDao : MappedRepositoryBase<ShopDto, ShopModel>, IShopService
    {
        public ShopDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
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
