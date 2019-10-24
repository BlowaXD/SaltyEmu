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
    public class MapNpcDao : MappedRepositoryBase<MapNpcDto, MapNpcModel>, IMapNpcService
    {
        private readonly Dictionary<long, MapNpcDto[]> _npcs;

        public MapNpcDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
        {
            _npcs = new Dictionary<long, MapNpcDto[]>(Get().GroupBy(s => s.MapId).ToDictionary(s => (long)s.Key, s => s.ToArray()));
        }


        public IEnumerable<MapNpcDto> GetByMapId(long mapId)
        {
            try
            {
                if (!_npcs.TryGetValue(mapId, out MapNpcDto[] dtos))
                {
                    dtos = DbSet.Where(s => s.MapId == mapId).AsEnumerable().Select(Mapper.Map<MapNpcDto>).ToArray();
                    _npcs[mapId] = dtos;
                }

                return dtos;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<MapNpcDto>> GetByMapIdAsync(long mapId)
        {
            try
            {
                if (!_npcs.TryGetValue(mapId, out MapNpcDto[] dtos))
                {
                    dtos = (await DbSet.Where(s => s.MapId == mapId).ToArrayAsync()).Select(Mapper.Map<MapNpcDto>).ToArray();
                    _npcs[mapId] = dtos;
                }

                return dtos;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }
        }
    }
}