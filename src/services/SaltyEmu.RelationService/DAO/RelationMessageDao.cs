using ChickenAPI.Data.Relations;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService.DAO
{
    public class RelationMessageDao : GenericRedisCacheClient<RelationMessageDto>
    {
        public RelationMessageDao(RedisConfiguration conf) : base(conf)
        {
        }
    }
}