using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Entities
{
    [PacketHeader("out")]
    public class OutPacket : PacketBase
    {
        public VisualType Type { get; set; }

        public long EntityId { get; set; }
    }
}