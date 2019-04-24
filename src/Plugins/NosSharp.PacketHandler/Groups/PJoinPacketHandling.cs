using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Groups.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.Old.Game.Server.Group;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Groups
{
    public class PJoinPacketHandling : GenericGamePacketHandlerAsync<PJoinPacket>
    {
        private readonly IPlayerManager _manager;
        private readonly IGroupManager _groupManager;

        public PJoinPacketHandling(IPlayerManager manager, IGroupManager groupManager)
        {
            _manager = manager;
            _groupManager = groupManager;
        }

        protected override async Task Handle(PJoinPacket packet, IPlayerEntity player)
        {
            switch (packet.RequestType)
            {
                case PJoinPacketType.Requested:
                case PJoinPacketType.Invited:
                    await player.EmitEventAsync(new GroupInvitationSendEvent
                    {
                        Target = _manager.GetPlayerByCharacterId(packet.CharacterId)
                    });
                    break;
                case PJoinPacketType.Accepted:
                    GroupInvitDto group = _groupManager.GetPendingInvitationsByCharacterId(player.Id)?.FirstOrDefault(s => s.Target == player);

                    if (group == null)
                    {
                        return;
                    }

                    await player.EmitEventAsync(new GroupInvitationAcceptEvent
                    {
                        Invitation = group
                    });
                    break;
                case PJoinPacketType.Declined:
                    GroupInvitDto invitation = _groupManager.GetPendingInvitationsByCharacterId(player.Id)?.FirstOrDefault(s => s.Target == player);

                    if (invitation == null)
                    {
                        return;
                    }

                    await player.EmitEventAsync(new GroupInvitationRefuseEvent()
                    {
                        Invitation = invitation
                    });
                    break;
                case PJoinPacketType.Sharing:
                    break;
                case PJoinPacketType.AcceptedShare:
                    break;
                case PJoinPacketType.DeclinedShare:
                    break;
            }
        }
    }
}