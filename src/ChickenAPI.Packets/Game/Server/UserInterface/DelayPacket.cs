using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.UserInterface
{
    [PacketHeader("delay")]
    public class DelayPacket : PacketBase
    {
        [PacketIndex(0)]
        public int Delay { get; set; }

        [PacketIndex(1)]
        public DelayPacketType Type { get; set; }

        [PacketIndex(2)]
        public string Argument { get; set; }
    }
}