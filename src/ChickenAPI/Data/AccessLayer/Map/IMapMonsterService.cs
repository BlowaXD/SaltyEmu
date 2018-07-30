using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Map;

namespace ChickenAPI.Data.AccessLayer.Map
{
    public interface IMapMonsterService : IMappedRepository<MapMonsterDto>
    {
        IEnumerable<MapMonsterDto> GetByMapId(long mapId);
        Task<IEnumerable<MapMonsterDto>> GetByMapIdAsync(long mapId);
    }
}