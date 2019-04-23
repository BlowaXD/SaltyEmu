using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.UserInterface
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