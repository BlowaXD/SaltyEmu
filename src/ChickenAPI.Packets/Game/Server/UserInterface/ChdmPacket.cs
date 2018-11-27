using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.UserInterface
{
    [PacketHeader("ch_dm")]
    public class ChDMPacket : PacketBase
    {
        [PacketIndex(0)]
        public int Maxhp { get; set; }

        [PacketIndex(1)]
        public int AngelDMG { get; set; }

        [PacketIndex(2)]
        public int DemonDMG { get; set; }

        [PacketIndex(3)]
        public int Time { get; set; }
    }
}