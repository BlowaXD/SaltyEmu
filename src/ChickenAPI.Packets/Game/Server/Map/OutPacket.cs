using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Map
{
    [PacketHeader("out")]
    public class OutPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }
    }
}
