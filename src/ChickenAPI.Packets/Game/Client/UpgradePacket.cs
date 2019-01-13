using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("up_gr")]
    public class UpgradePacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public UpgradePacketType UpgradeType { get; set; }

        [PacketIndex(1)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(2)]
        public byte Slot { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public InventoryType? InventoryType2 { get; set; }

        [PacketIndex(4, IsOptional = true)]
        public byte? Slot2 { get; set; }

        [PacketIndex(5, IsOptional = true)]
        public InventoryType? CellonInventoryType { get; set; }

        [PacketIndex(6, IsOptional = true)]
        public byte? CellonSlot { get; set; }

        #endregion Properties
    }
}