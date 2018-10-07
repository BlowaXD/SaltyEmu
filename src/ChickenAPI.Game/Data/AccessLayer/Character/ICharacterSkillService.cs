using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterSkillService : ISynchronizedRepository<CharacterSkillDto>
    {
        IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId);
        Task<IEnumerable<CharacterSkillDto>> GetByCharacterIdAsync(long characterId);
    }
}