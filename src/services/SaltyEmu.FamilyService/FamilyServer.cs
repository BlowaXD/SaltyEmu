using System.Threading.Tasks;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;

namespace SaltyEmu.FamilyService
{
    public class FamilyServer : MqttIpcServer<FamilyServer>
    {
        public FamilyServer(MqttServerConfigurationBuilder builder) : base(builder)
        {
        }

        public new async Task<FamilyServer> InitializeAsync()
        {
            await base.InitializeAsync();
            return this;
        }
    }
}