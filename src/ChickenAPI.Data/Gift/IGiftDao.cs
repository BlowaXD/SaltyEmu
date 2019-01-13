using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Gift
{
    public interface IGiftDao : ISynchronizedRepository<GiftDto>
    {
        Task<IEnumerable<GiftDto>> GetPendingGiftsByCharacterId(long characterId);
    }
}