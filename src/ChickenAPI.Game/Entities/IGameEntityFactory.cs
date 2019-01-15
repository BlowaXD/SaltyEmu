using System.Threading.Tasks;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;

namespace ChickenAPI.Game.Entities
{
    public interface IGameEntityFactory
    {
        Task<IPlayerEntity> CreateNewPlayerEntityAsync(ISession session, long characterId);

        Task<IMateEntity> CreateNewMateEntityAsync(IPlayerEntity owner, CharacterMateDto dto);

        Task<IMateEntity> CreateNewMateEntityAsync(IPlayerEntity owner, long characterMateId);

        Task<INpcEntity> CreateNpcEntityAsync(long entityId, long npcMonsterId);

        Task<IMonsterEntity> CreateMonsterEntityAsync(long entityId, long npcMonsterId);
        Task<IMonsterEntity> CreateMonsterEntityAsync(MapMonsterDto mapMonster);
    }
}