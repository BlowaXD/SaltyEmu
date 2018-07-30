using AutoMapper;
using ChickenAPI.Data.AccessLayer.Map;
using ChickenAPI.Data.TransferObjects.Map;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Models.Map;
using NosSharp.DatabasePlugin.Services.Base;

namespace NosSharp.DatabasePlugin.Services.Map
{
    public class MapDao : MappedRepositoryBase<MapDto, MapModel>, IMapService
    {
        public MapDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}