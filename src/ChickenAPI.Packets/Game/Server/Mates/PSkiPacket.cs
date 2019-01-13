using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Mates
{
    [PacketHeader("pski")]
    public class PSkiPacket
    {
        [PacketIndex(0)]
        public long FirstSkill { get; set; }

        [PacketIndex(1)]
        public long SecondSkill { get; set; }

        [PacketIndex(2)]
        public long ThirdSkill { get; set; }
    }
}