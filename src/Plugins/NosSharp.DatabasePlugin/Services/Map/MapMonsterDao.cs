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
    public class MapMonsterDao : MappedRepositoryBase<MapMonsterDto, MapMonsterModel>, IMapMonsterService
    {
        private readonly Dictionary<short, MapMonsterDto[]> _monsters;

        public MapMonsterDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
        {
            _monsters = new Dictionary<short, MapMonsterDto[]>(Get().GroupBy(s => s.MapId).ToDictionary(dtos => dtos.Key, dtos => dtos.ToArray()));
        }

        public IEnumerable<MapMonsterDto> GetByMapId(long mapId)
        {
            try
            {
                if (!_monsters.TryGetValue((short)mapId, out MapMonsterDto[] items))
                {
                    items = DbSet.Where(s => s.MapId == mapId).ToArray().Select(Mapper.Map<MapMonsterDto>).ToArray();
                    _monsters[(short)mapId] = items;
                }

                return items;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }
        }

        public async Task<IEnumerable<MapMonsterDto>> GetByMapIdAsync(long mapId)
        {
            try
            {
                if (!_monsters.TryGetValue((short)mapId, out MapMonsterDto[] items))
                {
                    items = (await DbSet.Where(s => s.MapId == mapId).ToArrayAsync()).Select(Mapper.Map<MapMonsterDto>).ToArray();
                    _monsters[(short)mapId] = items;
                }

                return items;
            }
            catch (Exception e)
            {
                Log.Error("[GET_BY_MAP_ID]", e);
                throw;
            }
        }
    }
}