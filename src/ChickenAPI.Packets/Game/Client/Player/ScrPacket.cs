using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.Player
{
    [PacketHeader("scr")]
    public class ScrPacket : PacketBase
    {
        [PacketIndex(0)]
        public int Unknow1 { get; set; }

        [PacketIndex(1)]
        public int Unknow2 { get; set; }

        [PacketIndex(2)]
        public int Unknow3 { get; set; }

        [PacketIndex(3)]
        public int Unknow4 { get; set; }

        [PacketIndex(4)]
        public int Unknow5 { get; set; }

        [PacketIndex(5)]
        public int Unknow6 { get; set; }
    }
}