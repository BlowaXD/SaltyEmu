using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
{
    [PacketHeader("npinfo")]
    public class NpInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public long UnKnow { get; set; }
    }
}