using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("out")]
    public class OutPacketBase : PacketBase
    {
        public VisualType Type { get; set; }

        public long EntityId { get; set; }
    }
}