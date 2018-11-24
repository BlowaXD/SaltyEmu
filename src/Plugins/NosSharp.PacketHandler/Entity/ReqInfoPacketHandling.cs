using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.ReqInfo.Events;
using ChickenAPI.Packets.Game.Server.Entities;

namespace NosSharp.PacketHandler.Entity
{
    public class ReqInfoPacketHandling
    {
        public static void ReqInfoPacket(ReqInfoPacket packet, IPlayerEntity player)
        {
            if (packet.MateVNum.HasValue)
            {
                player.EmitEvent(new ReqInfoEvent
                {
                    MateVNum = packet.MateVNum,
                    TargetVNum = packet.TargetVNum,
                    ReqType = packet.ReqType
                });
                return;
            }

            player.EmitEvent(new ReqInfoEvent
            {
                TargetVNum = packet.TargetVNum,
                ReqType = packet.ReqType
            });
        }
    }
}