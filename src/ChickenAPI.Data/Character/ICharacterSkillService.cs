using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Character
{
    public interface ICharacterSkillService : ISynchronizedRepository<CharacterSkillDto>
    {
        IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId);
        Task<IEnumerable<CharacterSkillDto>> GetByCharacterIdAsync(long characterId);
    }
}