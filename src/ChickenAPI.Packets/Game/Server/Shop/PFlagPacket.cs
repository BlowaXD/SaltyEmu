using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Shop
{
    [PacketHeader("pflag")]
    public class PFlagPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType EntityType { get; set; }

        [PacketIndex(1)]
        public long EntityId { get; set; }

        [PacketIndex(2)]
        public long ShopId { get; set; }
    }
}