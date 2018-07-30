using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Data.AccessLayer.NpcMonster;
using ChickenAPI.Data.AccessLayer.Shop;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.Data.TransferObjects.NpcMonster;
using ChickenAPI.Data.TransferObjects.Shop;
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
                return DbSet.Where(s => s.MapNpcId == mapNpcId).ToList().Select(Mapper.Map<ShopDto>);
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
                return (await DbSet.Where(s => s.MapNpcId == mapNpcId).ToListAsync()).Select(Mapper.Map<ShopDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_NPC_ID]", e);
                throw;
            }
        }
    }
}
