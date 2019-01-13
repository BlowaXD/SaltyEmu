using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Relations
{
    public interface IRelationInvitationDao : ISynchronizedRepository<RelationInvitationDto>
    {
        Task<IEnumerable<RelationInvitationDto>> GetPendingRelationInvitationsByCharacterId(long characterId);
    }
}