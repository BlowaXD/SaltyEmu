using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Inventory
{
    [PacketHeader("u_i")]
    public class UiPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(3)]
        public byte InventorySlot { get; set; }

        [PacketIndex(4)]
        public byte Unknown2 { get; set; } //Seems to be always 0 - using accountbound costumes and accepting the dialog sends: "#u_i^1^1^7^0^1"

        [PacketIndex(5)]
        public byte Unknown3 { get; set; } //Seems to be always 0
    }
}