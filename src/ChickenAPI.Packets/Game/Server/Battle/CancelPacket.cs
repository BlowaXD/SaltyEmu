using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.QuickList.Battle
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