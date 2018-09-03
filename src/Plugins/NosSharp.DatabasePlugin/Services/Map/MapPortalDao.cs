using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.TransferObjects.Map;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Map;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Map
{
    public class MapPortalDao : MappedRepositoryBase<PortalDto, MapPortalModel>, IPortalService
    {
        public MapPortalDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public IEnumerable<PortalDto> GetByMapId(long mapId)
        {
            try
            {
                return DbSet.Where(s => s.SourceMapId == mapId).ToList().Select(Mapper.Map<PortalDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<PortalDto>> GetByMapIdAsync(long mapId)
        {
            try
            {
                return (await DbSet.Where(s => s.SourceMapId == mapId).ToListAsync()).Select(Mapper.Map<PortalDto>);
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }
        }
    }
}