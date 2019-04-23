using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Server.Player
{
    [PacketHeader("rage")]
    public class RagePacket : PacketBase
    {
        [PacketIndex(0)]
        public long RagePoints { get; set; }

        [PacketIndex(1)]
        public long RagePointsMax { get; set; }
    }
}