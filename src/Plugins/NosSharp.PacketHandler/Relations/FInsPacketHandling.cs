using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Relations;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Relations.Events;
using ChickenAPI.Packets.ClientPackets.Relations;
using ChickenAPI.Packets.Enumerations;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Relations
{
    public class FInsPacketHandling : GenericGamePacketHandlerAsync<FinsPacket>
    {
        public FInsPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(FinsPacket packet, IPlayerEntity player)
        {
            switch (packet.Type)
            {
                case FinsPacketType.Accepted:
                case FinsPacketType.Rejected:
                    await player.EmitEventAsync(new RelationInvitationProcessEvent
                    {
                        Type = packet.Type == FinsPacketType.Accepted ? RelationInvitationProcessType.Accept : RelationInvitationProcessType.Refuse,
                        TargetCharacterId = packet.CharacterId
                    });
                    break;
                default:
                    // packet modifed
                    return;
            }
        }
    }
}