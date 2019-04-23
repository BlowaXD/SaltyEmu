using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Shops
{
    [PacketHeader("m_shop")]
    public class MShopPacket : PacketBase
    {
        [PacketIndex(0)]
        public MShopPacketType Type { get; set; }

        /// <summary>
        ///     Todo write a better way to handle that
        /// </summary>
        [PacketIndex(1, SerializeToEnd = true)]
        public string PacketData { get; set; }
    }
}