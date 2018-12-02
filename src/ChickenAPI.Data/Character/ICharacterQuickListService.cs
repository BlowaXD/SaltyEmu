using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Character
{
    public interface ICharacterQuickListService : ISynchronizedRepository<CharacterQuicklistDto>
    {
        IEnumerable<CharacterQuicklistDto> GetByCharacterId(long characterId);

        Task<IEnumerable<CharacterQuicklistDto>> GetByCharacterIdAsync(long characterId);
    }
}