using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("put")]
    public class PutPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1)]
        public short InventorySlot { get; set; }

        [PacketIndex(2)]
        public short Amount { get; set; }
    }
}
