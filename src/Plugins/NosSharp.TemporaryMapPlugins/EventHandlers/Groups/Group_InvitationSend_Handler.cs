using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Groups.Events;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Groups;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_InvitationSend_Handler : GenericEventPostProcessorBase<GroupInvitationSendEvent>
    {
        private readonly IPlayerManager _playerManager;
        private readonly IGroupManager _groupManager;


        public Group_InvitationSend_Handler(ILogger log, IPlayerManager playerManager, IGroupManager groupManager) : base(log)
        {
            _playerManager = playerManager;
            _groupManager = groupManager;
        }

        protected override async Task Handle(GroupInvitationSendEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            IPlayerEntity sender = _playerManager.GetPlayerByCharacterId(e.Sender.Id);
            IPlayerEntity target = _playerManager.GetPlayerByCharacterId(e.Target.Character.Id);
            if (target == null || sender == null)
            {
                return;
            }

            _groupManager.CreateInvitation(sender, target);

            await player.SendChatMessageAsync(PlayerMessages.GROUP_PLAYER_X_INVITED_TO_YOUR_GROUP, SayColorType.Yellow);
            PacketBase acceptPacket = new PjoinPacket { CharacterId = target.Id, RequestType = GroupRequestType.Accepted };
            PacketBase refusePacket = new PjoinPacket { CharacterId = target.Id, RequestType = GroupRequestType.Declined };
            string question = target.GetLanguageFormat(PlayerMessages.GROUP_PLAYER_X_INVITED_YOU_TO_JOIN_HIS_GROUP, sender.Character.Name);
            await target.SendDialog(acceptPacket, refusePacket, question);
        }
    }
}