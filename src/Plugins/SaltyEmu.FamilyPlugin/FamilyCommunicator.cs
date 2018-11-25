using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.FamilyPlugin
{
    public class FamilyCommunicator : MqttClientCommunicator
    {
        // todo
        public FamilyCommunicator(RabbitMqConfiguration config, IIpcSerializer serializer) : base(config, serializer)
        {
        }
    }
}