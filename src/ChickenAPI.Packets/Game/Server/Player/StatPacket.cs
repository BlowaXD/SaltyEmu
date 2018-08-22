using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Player
{
    [PacketHeader("stat")]
    public class StatPacket : PacketBase
    {
        [PacketIndex(0)]
        public long Hp { get; set; }

        [PacketIndex(1)]
        public long HpMax { get; set; }

        [PacketIndex(2)]
        public long Mp { get; set; }

        [PacketIndex(3)]
        public long MpMax { get; set; }

        [PacketIndex(4)]
        public long Unknown { get; set; } // seems to be 0

        [PacketIndex(5)]
        public double CharacterOption { get; set; }
    }
}