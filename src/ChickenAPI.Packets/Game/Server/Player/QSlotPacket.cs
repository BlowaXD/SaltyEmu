using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Player
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