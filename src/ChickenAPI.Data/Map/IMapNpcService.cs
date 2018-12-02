using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Map
{
    public interface IMapNpcService : IMappedRepository<MapNpcDto>
    {
        IEnumerable<MapNpcDto> GetByMapId(long mapId);
        Task<IEnumerable<MapNpcDto>> GetByMapIdAsync(long mapId);
    }
}