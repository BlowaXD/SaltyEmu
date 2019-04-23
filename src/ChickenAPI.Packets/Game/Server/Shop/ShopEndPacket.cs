using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Shop
{
    [PacketHeader("shop_end")]
    public class ShopEndPacket : PacketBase
    {
        [PacketIndex(0)]
        public ShopEndPacketType Type { get; set; }
    }
}