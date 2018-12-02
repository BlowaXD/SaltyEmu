using System.Threading.Tasks;

namespace ChickenAPI.Data.Character
{
    public interface ICharacterMateService : IMappedRepository<CharacterMateDto>
    {
        /// <summary>
        ///     Will get all <see cref="CharacterMateDto" /> owned by the <see cref="CharacterDto" /> with the given id
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        CharacterMateDto[] GetMatesByCharacterId(long characterId);

        /// <summary>
        ///     Will get all <see cref="CharacterMateDto" /> owned by the <see cref="CharacterDto" /> with the given id
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        Task<CharacterMateDto[]> GetMatesByCharacterIdAsync(long characterId);
    }
}