using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Features.Quicklist
{
    [PacketHeader("qslot")]
    public class QSlotPacket : PacketBase
    {
        [PacketIndex(0)]
        public long Slot { get; set; }
        
        [PacketIndex(1)]
        public string Content { get; set; }
    }
}