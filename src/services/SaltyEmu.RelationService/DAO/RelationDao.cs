using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Relations;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService.DAO
{
    public class RelationDao : GenericRedisCacheClient<RelationDto>, IRelationDao
    {
        public RelationDao(RedisConfiguration conf) : base(conf)
        {
        }

        public async Task<IEnumerable<RelationDto>> GetRelationsByCharacterId(long characterId)
        {
            Log.Info($"GetRelationsByCharacterId({characterId})");
            return (await GetAsync()).Where(s => s.OwnerId == characterId);
        }
    }
}