using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Shops
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