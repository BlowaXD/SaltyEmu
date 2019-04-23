using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.UserInterface
{
    [PacketHeader("guri")]
    public class GuriPacket : PacketBase
    {
        [PacketIndex(0)]
        public GuriPacketType Type { get; set; }

        [PacketIndex(1)]
        public uint Unknown { get; set; } // seems to be a visual type or something

        [PacketIndex(2)]
        public long EntityId { get; set; }

        [PacketIndex(3)]
        public uint Value { get; set; }
    }
}