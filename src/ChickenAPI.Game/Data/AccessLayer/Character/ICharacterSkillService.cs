using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Game.Data.TransferObjects.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterSkillService : ISynchronizedRepository<CharacterSkillDto>
    {
        IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId);
        Task<IEnumerable<CharacterSkillDto>> GetByCharacterIdAsync(long characterId);
    }
}