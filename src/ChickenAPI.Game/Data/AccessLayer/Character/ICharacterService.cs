using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;

namespace ChickenAPI.Game.Data.AccessLayer.Character
{
    public interface ICharacterService : IMappedRepository<CharacterDto>
    {
        /// <summary>
        ///     Returns all the <see cref="CharacterDto" /> that are in <see cref="CharacterState.Active" /> state
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IEnumerable<CharacterDto> GetActiveByAccountId(long accountId);


        /// <summary>
        ///     Asynchronously returns all the <see cref="CharacterDto" /> that are in <see cref="CharacterState.Active" /> state
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<IEnumerable<CharacterDto>> GetActiveByAccountIdAsync(long accountId);

        /// <summary>
        ///     Returns the <see cref="CharacterState.Active" /> <see cref="CharacterDto" /> associated to its accountId & slot
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        CharacterDto GetByAccountIdAndSlot(long accountId, byte slot);


        /// <summary>
        ///     Asynchronously returns the <see cref="CharacterState.Active" /> <see cref="CharacterDto" /> associated to its
        ///     accountId & slot
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        Task<CharacterDto> GetByAccountIdAndSlotAsync(long accountId, byte slot);


        /// <summary>
        ///     Asynchronously returns the <see cref="CharacterState.Active" /> <see cref="CharacterDto" /> associated to its name
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        Task<CharacterDto> GetActiveByNameAsync(string characterName);

        /// <summary>
        ///     Returns the base character that will be used at character creation
        ///     Easily configurable by a file
        /// </summary>
        /// <returns></returns>
        CharacterDto GetCreationCharacter();
    }
}