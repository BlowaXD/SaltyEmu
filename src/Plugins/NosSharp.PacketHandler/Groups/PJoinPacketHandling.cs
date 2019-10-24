using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Groups.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Groups;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Groups
{
    public class PJoinPacketHandling : GenericGamePacketHandlerAsync<PjoinPacket>
    {
        private readonly IPlayerManager _manager;
        private readonly IGroupManager _groupManager;


        public PJoinPacketHandling(ILogger log, IPlayerManager manager, IGroupManager groupManager) : base(log)
        {
            _manager = manager;
            _groupManager = groupManager;
        }

        protected override async Task Handle(PjoinPacket packet, IPlayerEntity player)
        {
            switch (packet.RequestType)
            {
                case GroupRequestType.Requested:
                case GroupRequestType.Invited:
                    await player.EmitEventAsync(new GroupInvitationSendEvent
                    {
                        Target = _manager.GetPlayerByCharacterId(packet.CharacterId)
                    });
                    break;
                case GroupRequestType.Accepted:
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
                case GroupRequestType.Declined:
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
                case GroupRequestType.Sharing:
                    break;
                case GroupRequestType.AcceptedShare:
                    break;
                case GroupRequestType.DeclinedShare:
                    break;
            }
        }
    }
}