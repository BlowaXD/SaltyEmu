using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Features.Shops.Packets
{
    [PacketHeader("shop")]
    public class ShopPacket : PacketBase
    {
        [PacketIndex(0)]
        public int Unknown { get; set; }

        [PacketIndex(1)]
        public long EntityId { get; set; }

        [PacketIndex(2)]
        public long ShopId { get; set; }

        [PacketIndex(3)]
        public byte MenuType { get; set; }

        [PacketIndex(4)]
        public byte ShopType { get; set; }

        [PacketIndex(5)]
        public string Name { get; set; }
    }
}