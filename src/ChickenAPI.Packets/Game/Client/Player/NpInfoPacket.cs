using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Player
{
    [PacketHeader("npinfo")]
    public class NpInfoPacket : PacketBase
    {
        [PacketIndex(0)]
        public long UnKnow { get; set; }
    }
}