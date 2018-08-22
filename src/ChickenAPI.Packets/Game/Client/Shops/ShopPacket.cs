using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
{
    [PacketHeader("shop")]
    public class ShopPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

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