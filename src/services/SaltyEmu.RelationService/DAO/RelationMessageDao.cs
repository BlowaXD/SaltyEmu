using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Relations;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService.DAO
{
    public class RelationMessageDao : GenericRedisCacheClient<RelationMessageDto>, IRelationMessageDao
    {
        public RelationMessageDao(RedisConfiguration conf, ILogger log) : base(conf, log)
        {
        }

        public async Task<IEnumerable<RelationMessageDto>> GetPendingRelationMessagesByCharacterId(long characterId)
        {
            return (await GetAsync()).Where(s => s.TargetId == characterId);
        }
    }
}