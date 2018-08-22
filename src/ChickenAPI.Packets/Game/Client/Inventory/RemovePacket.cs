namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("remove")]
    public class RemovePacket : PacketBase
    {
        [PacketIndex(0)]
        public byte InventorySlot { get; set; }

        [PacketIndex(1)]
        public long MateId { get; set; }
    }
}