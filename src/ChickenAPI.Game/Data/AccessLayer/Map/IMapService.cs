using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace ChickenAPI.Game.Data.AccessLayer.Map
{
    public interface IMapService : IMappedRepository<MapDto>
    {
    }
}