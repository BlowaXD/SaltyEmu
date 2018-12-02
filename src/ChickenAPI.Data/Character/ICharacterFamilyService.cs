using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChickenAPI.Data.Character
{
    public interface ICharacterFamilyService : IMappedRepository<CharacterFamilyDto>
    {
        CharacterFamilyDto GetCharacterFamilyByCharacterId(long characterId);
        Task<CharacterFamilyDto> GetCharacterFamilyByCharacterIdAsync(long characterId);

        IEnumerable<CharacterFamilyDto> GetCharacterFamilyDtosByFamilyId(long familyId);
        Task<IEnumerable<CharacterFamilyDto>> GetCharacterFamilyDtosByFamilyIdAsync(long familyId);
    }
}