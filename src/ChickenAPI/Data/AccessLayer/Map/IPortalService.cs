using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Map;

namespace ChickenAPI.Data.AccessLayer.Map
{
    /// <inheritdoc />
    public interface IPortalService : IMappedRepository<PortalDto>
    {
        IEnumerable<PortalDto> GetByMapId(long mapId);

        Task<IEnumerable<PortalDto>> GetByMapIdAsync(long mapId);
    }
}