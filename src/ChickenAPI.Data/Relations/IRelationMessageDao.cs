using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Relations
{
    public interface IRelationMessageDao : ISynchronizedRepository<RelationMessageDto>
    {
        Task<IEnumerable<RelationMessageDto>> GetPendingRelationMessagesByCharacterId(long characterId);
    }
}