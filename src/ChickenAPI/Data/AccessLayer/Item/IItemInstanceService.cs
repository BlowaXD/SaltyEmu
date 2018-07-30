using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.Data.TransferObjects.Item;
using ChickenAPI.Enums;

namespace ChickenAPI.Data.AccessLayer.Item
{
    public interface IItemInstanceService : ISynchronizedRepository<ItemInstanceDto>
    {
        /// <summary>
        /// Returns the inventory that is given at the character creation depending on its authority type
        /// </summary>
        /// <returns></returns>
        IEnumerable<(long, ItemDto)> GetBaseInventory(AuthorityType authorityType);

        /// <summary>
        /// Returns all the weared gear of the character by character's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<ItemInstanceDto> GetWearByCharacterId(long id);

        /// <summary>
        ///     Will return all the weared gear in the <see cref="CharacterDto" /> inventory by its id
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        Task<IEnumerable<ItemInstanceDto>> GetWearByCharacterIdAsync(long characterId);


        IEnumerable<ItemInstanceDto> GetByCharacterId(long characterId);

        Task<IEnumerable<ItemInstanceDto>> GetByCharacterIdAsync(long characterId);
    }
}