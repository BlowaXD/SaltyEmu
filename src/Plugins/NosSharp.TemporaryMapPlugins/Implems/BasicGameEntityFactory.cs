using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;

namespace ChickenAPI.Game.Entities
{
    public class BasicGameEntityFactory : IGameEntityFactory
    {
        private readonly ICharacterService _characterService;
        private readonly ICharacterSkillService _characterSkill;
        private readonly ICharacterQuickListService _characterQuicklist;
        private readonly ICharacterMateService _characterMate;
        private readonly INpcMonsterService _npcMonsterService;
        private readonly IMapMonsterService _monsterService;

        public BasicGameEntityFactory(
            ICharacterService characterService, ICharacterSkillService characterSkillService,
            ICharacterQuickListService characterQuicklist, ICharacterMateService characterMate,
            INpcMonsterService npcMonsterService, IMapMonsterService monsterService)
        {
            _characterService = characterService;
            _characterSkill = characterSkillService;
            _characterQuicklist = characterQuicklist;
            _characterMate = characterMate;
            _npcMonsterService = npcMonsterService;
            _monsterService = monsterService;
        }

        public async Task<IPlayerEntity> CreateNewPlayerEntityAsync(ISession session, long characterId)
        {
            CharacterDto character = await _characterService.GetByIdAsync(characterId);
            IEnumerable<CharacterSkillDto> skills = await _characterSkill.GetByCharacterIdAsync(characterId);
            IEnumerable<CharacterQuicklistDto> quicklist = await _characterQuicklist.GetByCharacterIdAsync(characterId);
            return new PlayerEntity(session, character, skills, quicklist);
        }

        public async Task<IMateEntity> CreateNewMateEntityAsync(IPlayerEntity owner, CharacterMateDto dto)
        {
            NpcMonsterDto npcMonster = await _npcMonsterService.GetByIdAsync(dto.NpcMonsterId);
            return new MateEntity(owner, dto, npcMonster);
        }

        public async Task<IMateEntity> CreateNewMateEntityAsync(IPlayerEntity owner, long characterMateId)
        {
            CharacterMateDto mate = await _characterMate.GetByIdAsync(characterMateId);
            return await CreateNewMateEntityAsync(owner, mate);
        }

        public Task<INpcEntity> CreateNpcEntityAsync(long entityId, long npcMonsterId)
        {
            return null;
        }

        public async Task<IMonsterEntity> CreateMonsterEntityAsync(long entityId, long npcMonsterId)
        {
            return await CreateMonsterEntityAsync(await _monsterService.GetByIdAsync(npcMonsterId));
        }

        public Task<IMonsterEntity> CreateMonsterEntityAsync(MapMonsterDto mapMonster)
        {
            // todo implement that
            var skills = new List<NpcMonsterSkillDto>();
            return Task.FromResult(new MonsterEntity(mapMonster, skills) as IMonsterEntity);
        }
    }
}