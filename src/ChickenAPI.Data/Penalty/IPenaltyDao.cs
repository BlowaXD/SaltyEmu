using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Penalty
{
    public interface IPenaltyDao : ISynchronizedRepository<PenaltyDto>
    {
        Task<IEnumerable<PenaltyDto>> GetActualPenaltiesByCharacterId(long characterId);
    }
}