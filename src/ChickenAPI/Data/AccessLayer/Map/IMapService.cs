using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Map;

namespace ChickenAPI.Data.AccessLayer.Map
{
    public interface IMapService : IMappedRepository<MapDto>
    {
    }
}