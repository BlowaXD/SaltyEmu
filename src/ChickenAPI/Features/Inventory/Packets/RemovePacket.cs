using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Features.Inventory.Packets
{
    [PacketHeader("remove")]
    public class RemovePacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public byte InventorySlot { get; set; }

        [PacketIndex(1)]
        public long MateId { get; set; }

        #endregion
    }
}