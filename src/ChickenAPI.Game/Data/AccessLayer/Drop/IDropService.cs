using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Drop;

namespace ChickenAPI.Game.Data.AccessLayer.Drop
{
    public interface IDropService : IMappedRepository<DropDto>
    {
        Task<IEnumerable<DropDto>> GetByMapNpcMonsterIdAsync(long mapNpcMonsterId);
    }
}