using AutoMapper;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.Data.AccessLayer.Map;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.Map;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.Map
{
    public class MapDao : MappedRepositoryBase<MapDto, MapModel>, IMapService
    {
        public MapDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}