using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("qslot")]
    public class QSlotPacket : PacketBase
    {
        [PacketIndex(0)]
        public long Slot { get; set; }

        [PacketIndex(1)]
        public string Data { get; set; }
    }
}