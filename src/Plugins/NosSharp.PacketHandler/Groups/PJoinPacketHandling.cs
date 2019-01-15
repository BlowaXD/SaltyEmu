using System.Threading.Tasks;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Groups.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Packets.Game.Server.Group;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Groups
{
    public class PJoinPacketHandling : GenericGamePacketHandlerAsync<PJoinPacket>
    {
        private readonly IPlayerManager _manager;

        public PJoinPacketHandling(IPlayerManager manager) => _manager = manager;

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
                    break;
                case PJoinPacketType.Declined:
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