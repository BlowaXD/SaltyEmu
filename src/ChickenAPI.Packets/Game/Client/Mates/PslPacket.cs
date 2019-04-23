using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Mates
{
    [PacketHeader("psl")]
    public class PslPacket : PacketBase
    {
        [PacketIndex(0)]
        public PslPacketType Type { get; set; }
    }
}