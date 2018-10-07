using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Map;

namespace ChickenAPI.Game.Data.AccessLayer.Map
{
    /// <inheritdoc />
    public interface IPortalService : IMappedRepository<PortalDto>
    {
        IEnumerable<PortalDto> GetByMapId(long mapId);

        Task<IEnumerable<PortalDto>> GetByMapIdAsync(long mapId);
    }
}