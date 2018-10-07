using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterFamilyService : IMappedRepository<CharacterFamilyDto>
    {
        CharacterFamilyDto GetCharacterFamilyByCharacterId(long characterId);
        Task<CharacterFamilyDto> GetCharacterFamilyByCharacterIdAsync(long characterId);

        IEnumerable<CharacterFamilyDto> GetCharacterFamilyDtosByFamilyId(long familyId);
        Task<IEnumerable<CharacterFamilyDto>> GetCharacterFamilyDtosByFamilyIdAsync(long familyId);
    }
}