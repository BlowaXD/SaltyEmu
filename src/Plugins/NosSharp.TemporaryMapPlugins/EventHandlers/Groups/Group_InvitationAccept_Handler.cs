using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Groups.Events;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game._i18n;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_InvitationAccept_Handler : GenericEventPostProcessorBase<GroupInvitationAcceptEvent>
    {
        private readonly IGroupManager _groupManager;

        public Group_InvitationAccept_Handler(IGroupManager groupManager)
        {
            _groupManager = groupManager;
        }

        protected override async Task Handle(GroupInvitationAcceptEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            _groupManager.AcceptInvitation(e.Invitation);
            if (!player.HasGroup)
            {
                return;
            }

            await player.SendChatMessageFormat(PlayerMessages.GROUP_YOU_JOINED_GROUP_OF_PLAYER_X, SayColorType.Yellow, player.Group.Leader.Character.Name);

            foreach (IPlayerEntity member in player.Group.Players)
            {
                await member.ActualizeUiGroupIcons();
                await member.ActualizeUiGroupStats();
                if (member != player)
                {
                    await member.SendChatMessageFormat(PlayerMessages.GROUP_PLAYER_X_JOINED_YOUR_GROUP, SayColorType.Yellow, player.Character.Name);
                }
            }
        }
    }
}