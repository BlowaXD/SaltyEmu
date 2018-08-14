using System.Collections.Generic;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterSkillService : ISynchronizedRepository<CharacterSkillDto>
    {
        IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId);
    }
}