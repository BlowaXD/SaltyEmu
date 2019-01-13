using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Mates
{
    [PacketHeader("psl")]
    public class PslPacket : PacketBase
    {
        [PacketIndex(0)]
        public PslPacketType Type { get; set; }
    }
}