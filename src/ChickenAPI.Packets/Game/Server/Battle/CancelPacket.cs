using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Battle
{
    [PacketHeader("cancel")]
    public class CancelPacket : PacketBase
    {
        [PacketIndex(0)]
        public CancelPacketType Type { get; set; }

        [PacketIndex(1)]
        public long TargetId { get; set; }
    }
}