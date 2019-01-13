using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Gift
{
    public interface IGiftService
    {
        Task<IEnumerable<GiftDto>> GetPendingGiftsByCharacterId(long characterId);

        Task SendGift(GiftDto gift);
    }
}