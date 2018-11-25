using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.FamilyPlugin
{
    public class Family : MqttIpcClient
    {
        // todo
        public Family(RabbitMqConfiguration config, IIpcSerializer serializer) : base(config, serializer)
        {
        }
    }
}