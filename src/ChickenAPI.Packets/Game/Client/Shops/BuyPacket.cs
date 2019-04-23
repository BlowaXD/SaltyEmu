using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Shops
{
    [PacketHeader("buy")]
    public class BuyPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public VisualType Type { get; set; }

        [PacketIndex(1)]
        public long OwnerId { get; set; }

        [PacketIndex(2)]
        public short Slot { get; set; }

        [PacketIndex(3)]
        public short Amount { get; set; }

        #endregion
    }
}