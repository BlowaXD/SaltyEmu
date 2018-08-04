namespace ChickenAPI.Game.Packets.Game.Client
{
    [PacketHeader("shopping")]
    public class ShoppingPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(3)]
        public int NpcId { get; set; }
    }
}