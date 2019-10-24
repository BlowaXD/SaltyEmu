using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Relations.Events;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.FriendsPlugin.Handlers
{
    internal class SendInvitationHandler : GenericAsyncRpcRequestHandler<SendInvitation>
    {
        public SendInvitationHandler(ILogger log) : base(log)
        {
        }

        protected override Task Handle(SendInvitation request)
        {
            if (!(request is SendInvitation sendInvitation))
            {
                return Task.CompletedTask;
            }

            var players = ChickenContainer.Instance.Resolve<IPlayerManager>();


            IPlayerEntity player = players.GetPlayerByCharacterId(sendInvitation.Invitation.TargetId);

            if (player == null)
            {
                return Task.CompletedTask;
            }

            return player.EmitEventAsync(new RelationInvitationReceiveEvent
            {
                Invitation = sendInvitation.Invitation
            });
        }
    }
}