using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Drops
{
    [PacketHeader("get")]
    public class GetPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public short CharacterId { get; set; }

        [PacketIndex(2)]
        public long DropId { get; set; }

        [PacketIndex(3)]
        public byte Unknown2 { get; set; } // seems to be always 0
    }
}