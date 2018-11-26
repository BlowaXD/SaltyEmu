using ChickenAPI.Core.IPC;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.FamilyService
{
    public class FamilyServer : MqttIpcServer<FamilyServer>
    {
        public FamilyServer(MqttServerConfigurationBuilder builder) : base(builder)
        {
        }
    }
}