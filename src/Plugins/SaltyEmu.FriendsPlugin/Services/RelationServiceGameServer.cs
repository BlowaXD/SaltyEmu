using System.Threading.Tasks;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.FriendsPlugin.Handlers;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.FriendsPlugin.Services
{
    public class RelationServiceGameServer : MqttIpcServer<RelationServiceGameServer>
    {
        public RelationServiceGameServer(MqttServerConfigurationBuilder builder) : base(builder)
        {
            RequestHandler.Register<SendInvitation>(new SendInvitationHandler().Handle);
        }

        public new async Task<RelationServiceGameServer> InitializeAsync()
        {
            await base.InitializeAsync();
            return this;
        }
    }
}