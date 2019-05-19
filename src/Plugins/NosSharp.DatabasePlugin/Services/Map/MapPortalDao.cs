using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Map;

namespace SaltyEmu.DatabasePlugin.Services.Map
{
    public class MapPortalDao : MappedRepositoryBase<PortalDto, MapPortalModel>, IPortalService
    {
        public MapPortalDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
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