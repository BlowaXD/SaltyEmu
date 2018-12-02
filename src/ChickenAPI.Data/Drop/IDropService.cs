using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Drop
{
    public interface IDropService : IMappedRepository<DropDto>
    {
        Task<IEnumerable<DropDto>> GetByMapNpcMonsterIdAsync(long mapNpcMonsterId);
    }
}