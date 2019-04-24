using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.ServerPackets.Entities;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Entity
{
    public class ReqInfoPacketHandling : GenericGamePacketHandlerAsync<ReqInfoPacket>
    {
        protected override async Task Handle(ReqInfoPacket packet, IPlayerEntity player)
        {
            if (packet.MateVNum.HasValue)
            {
                await player.EmitEventAsync(new ReqInfoEvent
                {
                    MateTransportId = packet.MateVNum,
                    TargetId = packet.TargetVNum,
                    ReqType = packet.ReqType
                });
                return;
            }

            await player.EmitEventAsync(new ReqInfoEvent
            {
                TargetId = packet.TargetVNum,
                ReqType = packet.ReqType
            });
        }
    }
}