using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterQuickListService : ISynchronizedRepository<CharacterQuicklistDto>
    {
        IEnumerable<CharacterQuicklistDto> GetByCharacterId(long characterId);

        Task<IEnumerable<CharacterQuicklistDto>> GetByCharacterIdAsync(long characterId);
    }
}