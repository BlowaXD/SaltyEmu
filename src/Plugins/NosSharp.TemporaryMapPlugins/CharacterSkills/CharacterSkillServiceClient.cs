using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Data.Character;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;

namespace SaltyEmu.BasicPlugin.CharacterSkills
{
    public class CharacterSkillServiceClient : SynchronizedRepositoryMqtt<CharacterSkillDto>, ICharacterSkillService
    {
        public CharacterSkillServiceClient(MqttClientConfigurationBuilder builder, IIpcClient client) : base(builder, client)
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