using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Inventory
{
    [PacketHeader("wear")]
    public class WearPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1)]
        public short ItemSlot { get; set; }
    }
}