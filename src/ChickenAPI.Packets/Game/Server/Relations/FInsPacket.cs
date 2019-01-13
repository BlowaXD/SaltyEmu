using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Relations
{
    [PacketHeader("fins")]
    public class FInsPacket : PacketBase
    {
        [PacketIndex(0)]
        public FInsPacketType Type { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }
    }
}