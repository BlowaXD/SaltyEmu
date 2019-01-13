using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Relations;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Relations.Events;
using ChickenAPI.Packets.Game.Server.Relations;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Relations
{
    public class FInsPacketHandling : GenericGamePacketHandlerAsync<FInsPacket>
    {
        protected override async Task Handle(FInsPacket packet, IPlayerEntity player)
        {
            switch (packet.Type)
            {
                case FInsPacketType.Accept:
                case FInsPacketType.Refuse:
                    await player.EmitEventAsync(new RelationInvitationProcessEvent
                    {
                        Type = packet.Type == FInsPacketType.Accept ? RelationInvitationProcessType.Accept : RelationInvitationProcessType.Refuse,
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