using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
{
    [PacketHeader("m_shop")]
    public class MShopPacket : PacketBase
    {
        [PacketIndex(0)]
        public MShopPacketType PacketType { get; set; }

        /// <summary>
        /// Todo write a better way to handle that
        /// </summary>
        [PacketIndex(1)]
        public string PacketData { get; set; }
    }
}