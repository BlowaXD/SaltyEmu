using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Map
{
    /// <inheritdoc />
    public interface IPortalService : IMappedRepository<PortalDto>
    {
        IEnumerable<PortalDto> GetByMapId(long mapId);

        Task<IEnumerable<PortalDto>> GetByMapIdAsync(long mapId);
    }
}