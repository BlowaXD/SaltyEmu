using AutoMapper;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Map;

namespace SaltyEmu.DatabasePlugin.Services.Map
{
    public class MapDao : MappedRepositoryBase<MapDto, MapModel>, IMapService
    {
        public MapDao(DbContext context, IMapper mapper, ILogger log) : base(context, mapper, log)
        {
        }
    }
}