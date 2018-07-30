namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("rmvobj")]
    public class RmvobjPacket : PacketBase
    {
        [PacketIndex(0)]
        public short Slot { get; set; }
    }
}
