using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace ChickenAPI.Game.Data.AccessLayer.Map
{
    /// <inheritdoc />
    public interface IPortalService : IMappedRepository<PortalDto>
    {
        IEnumerable<PortalDto> GetByMapId(long mapId);

        Task<IEnumerable<PortalDto>> GetByMapIdAsync(long mapId);
    }
}