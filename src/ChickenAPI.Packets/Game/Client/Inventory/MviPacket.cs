using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Inventory
{
    [PacketHeader("mvi")]
    public class MviPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1)]
        public short InventorySlot { get; set; }

        [PacketIndex(2)]
        public short Amount { get; set; }

        [PacketIndex(3)]
        public short DestinationSlot { get; set; }
    }
}