using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Data.AccessLayer;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Game.Data.TransferObjects.BCard;

namespace ChickenAPI.Game.Data.AccessLayer.BCard
{
    public interface IBCardService : IMappedRepository<BCardDto>
    {
        Task<BCardDto> GetByIdAndType(long id, BCardRelationType type);

        Task<IEnumerable<BCardDto>> GetBySkillIdAsync(long skillId);

        Task<IEnumerable<BCardDto>> GetByMapMonsterIdAsync(long monsterId);

        Task<IEnumerable<BCardDto>> GetByCardIdAsync(long cardId);

        Task<IEnumerable<BCardDto>> GetByItemIdAsync(long itemId);
    }
}