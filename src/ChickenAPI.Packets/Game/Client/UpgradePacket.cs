using ChickenAPI.Enums.Game.Entity;
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

        [PacketIndex(3)]
        public InventoryType? InventoryType2 { get; set; }

        [PacketIndex(4)]
        public byte? Slot2 { get; set; }

        [PacketIndex(5)]
        public InventoryType? CellonInventoryType { get; set; }

        [PacketIndex(6)]
        public byte? CellonSlot { get; set; }

        #endregion Properties
    }
}