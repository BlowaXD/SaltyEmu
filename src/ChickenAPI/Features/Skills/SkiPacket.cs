using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Game.Features.Skills
{
    [PacketHeader("ski")]
    public class SkiPacket : PacketBase
    {
        [PacketIndex(0)]
        public string SkiPacketContent { get; set; }
    }
}