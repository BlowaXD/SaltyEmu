using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.NpcMonster
{
    public interface INpcMonsterSkillService : IMappedRepository<NpcMonsterSkillDto>
    {
        Task<IEnumerable<NpcMonsterSkillDto>> GetByNpcMonsterIdsAsync(IEnumerable<long> ids);
    }
}