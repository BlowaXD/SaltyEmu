using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Map
{
    public interface IMapMonsterService : IMappedRepository<MapMonsterDto>
    {
        IEnumerable<MapMonsterDto> GetByMapId(long mapId);
        Task<IEnumerable<MapMonsterDto>> GetByMapIdAsync(long mapId);
    }
}