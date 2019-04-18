using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Data.Character;
using SaltyEmu.Communication.Communicators;

namespace SaltyEmu.BasicPlugin.CharacterSkills
{
    public class CharacterSkillServiceClient : SynchronizedRpcRepository<CharacterSkillDto>, ICharacterSkillService
    {
        public CharacterSkillServiceClient(IRpcClient client) : base(client)
        {
        }

        public IEnumerable<CharacterSkillDto> GetByCharacterId(long characterId)
        {
            return null;
        }

        public async Task<IEnumerable<CharacterSkillDto>> GetByCharacterIdAsync(long characterId)
        {
            return null;
        }
    }
}