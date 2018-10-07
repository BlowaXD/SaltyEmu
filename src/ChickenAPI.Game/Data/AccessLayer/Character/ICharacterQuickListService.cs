using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterQuickListService : ISynchronizedRepository<CharacterQuicklistDto>
    {
        IEnumerable<CharacterQuicklistDto> GetByCharacterId(long characterId);

        Task<IEnumerable<CharacterQuicklistDto>> GetByCharacterIdAsync(long characterId);
    }
}