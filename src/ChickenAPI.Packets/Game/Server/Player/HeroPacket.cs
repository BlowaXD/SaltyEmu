using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("hero")]
    public class HeroPacket : PacketBase
    {
        [PacketIndex(0, serializeToEnd: true)]
        public string Message { get; set; }
    }
}