using ChickenAPI.Data.Relations;
using SaltyEmu.Redis;

namespace SaltyEmu.RelationService.DAO
{
    public class RelationInvitationDao : GenericRedisCacheClient<RelationInvitationDto>
    {
        public RelationInvitationDao(RedisConfiguration conf) : base(conf)
        {
        }
    }
}