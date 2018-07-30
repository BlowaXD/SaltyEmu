using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Drop;

namespace ChickenAPI.Data.AccessLayer.Drop
{
    public interface IDropService : IMappedRepository<DropDto>
    {
        Task<IEnumerable<DropDto>> GetByMapNpcMonsterIdAsync(long mapNpcMonsterId);
    }
}