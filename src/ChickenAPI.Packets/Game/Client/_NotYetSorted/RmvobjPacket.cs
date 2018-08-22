using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client._NotYetSorted
{
    [PacketHeader("rmvobj")]
    public class RmvobjPacket : PacketBase
    {
        [PacketIndex(0)]
        public short Slot { get; set; }
    }
}