using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Drop;

namespace ChickenAPI.Game.Data.AccessLayer.Drop
{
    public interface IDropService : IMappedRepository<DropDto>
    {
        Task<IEnumerable<DropDto>> GetByMapNpcMonsterIdAsync(long mapNpcMonsterId);
    }
}