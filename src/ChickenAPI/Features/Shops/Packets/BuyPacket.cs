using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Features.Shops.Packets
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