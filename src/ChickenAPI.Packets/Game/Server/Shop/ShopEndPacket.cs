using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Shop
{
    [PacketHeader("shop_end")]
    public class ShopEndPacket : PacketBase
    {
        public ShopEndPacketType PacketType { get; set; }
    }
}