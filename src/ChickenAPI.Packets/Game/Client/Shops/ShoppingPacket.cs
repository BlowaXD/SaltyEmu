using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
{
    [PacketHeader("shopping")]
    public class ShoppingPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(1)]
        public int Unknown { get; set; }

        [PacketIndex(2)]
        public int Unknown1 { get; set; }

        [PacketIndex(3)]
        public int NpcId { get; set; }
    }
}