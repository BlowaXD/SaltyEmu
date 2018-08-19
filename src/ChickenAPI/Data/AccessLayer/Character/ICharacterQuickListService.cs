using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.TransferObjects.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterQuickListService : ISynchronizedRepository<CharacterQuicklistDto>
    {

        IEnumerable<CharacterQuicklistDto> LoadByCharacterId(long characterId);

        Task<IEnumerable<CharacterQuicklistDto>> LoadByCharacterIdAsync(long characterId);
    }
}