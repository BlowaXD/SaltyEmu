using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("get")]
    public class GetPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Unknown1 { get; set; } = 1;

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public long ItemId { get; set; }

        [PacketIndex(3)]
        public byte Unknown2 { get; set; } = 0;
    }
}