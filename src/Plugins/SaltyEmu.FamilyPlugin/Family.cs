using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.FamilyPlugin
{
    public class Family : MqttIpcClient<Family>
    {
        public Family(MqttClientConfigurationBuilder builder) : base(builder)
        {
        }
    }
}