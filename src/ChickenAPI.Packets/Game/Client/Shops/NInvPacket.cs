using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
{
    [PacketHeader("n_inv")]
    public class NInvPacket : PacketBase
    {
        [PacketIndex(0)]
        public int VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public byte Unknown { get; set; } = 0;

        [PacketIndex(3)]
        public int ShopType { get; set; }

        [PacketIndex(4)]
        public string ShopList { get; set; }
    }
}