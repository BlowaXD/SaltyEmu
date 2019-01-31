using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Relations;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService.DAO
{
    public class RelationInvitationDao : GenericRedisCacheClient<RelationInvitationDto>, IRelationInvitationDao
    {
        public RelationInvitationDao(RedisConfiguration conf) : base(conf)
        {
        }

        public async Task<IEnumerable<RelationInvitationDto>> GetPendingRelationInvitationsByCharacterId(long characterId)
        {
            return (await GetAsync()).Where(s => s.TargetId == characterId);
        }
    }
}