using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Entities
{
    [PacketHeader("char_sc")]
    public class CharScPacket : PacketBase
    {
        public VisualType VisualType { get; set; }

        public long VisualId { get; set; }

        public byte Size { get; set; }
    }
}