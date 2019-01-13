using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Relations
{
    public interface IRelationDao : ISynchronizedRepository<RelationDto>
    {
        Task<IEnumerable<RelationDto>> GetRelationsByCharacterId(long characterId);
    }
}