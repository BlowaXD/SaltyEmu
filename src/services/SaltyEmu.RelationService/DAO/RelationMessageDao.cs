using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Relations;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService.DAO
{
    public class RelationMessageDao : GenericRedisCacheClient<RelationMessageDto>, IRelationMessageDao
    {
        public RelationMessageDao(RedisConfiguration conf) : base(conf)
        {
        }

        public async Task<IEnumerable<RelationMessageDto>> GetPendingRelationMessagesByCharacterId(long characterId)
        {
            return (await GetAsync()).Where(s => s.TargetId == characterId);
        }
    }
}