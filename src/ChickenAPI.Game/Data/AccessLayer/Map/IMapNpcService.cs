using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace ChickenAPI.Game.Data.AccessLayer.Map
{
    public interface IMapNpcService : IMappedRepository<MapNpcDto>
    {
        IEnumerable<MapNpcDto> GetByMapId(long mapId);
        Task<IEnumerable<MapNpcDto>> GetByMapIdAsync(long mapId);
    }
}