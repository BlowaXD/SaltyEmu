using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Map;

namespace ChickenAPI.Data.AccessLayer.Map
{
    public interface IMapNpcService : IMappedRepository<MapNpcDto>
    {
        IEnumerable<MapNpcDto> GetByMapId(long mapId);
        Task<IEnumerable<MapNpcDto>> GetByMapIdAsync(long mapId);
    }
}