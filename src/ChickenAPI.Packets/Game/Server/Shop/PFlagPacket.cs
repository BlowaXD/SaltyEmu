using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Shop
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