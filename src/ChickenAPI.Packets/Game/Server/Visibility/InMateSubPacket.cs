using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Visibility
{
    [PacketHeader("in_mate_subpacket")]
    public class InMateSubPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public int HpPercentage { get; set; }

        [PacketIndex(1)]
        public int MpPercentage { get; set; }

        [PacketIndex(2)]
        public int Unknow { get; set; }

        [PacketIndex(3)]
        public int Faction { get; set; }

        [PacketIndex(4)]
        public int Unknow2 { get; set; }

        [PacketIndex(5)]
        public long OwnerId { get; set; }

        [PacketIndex(6)]
        public int Unknow3 { get; set; }

        [PacketIndex(7)]
        public int Unknow4 { get; set; }

        [PacketIndex(8)]
        public int Morph { get; set; }

        [PacketIndex(9)]
        public string Name { get; set; }

        [PacketIndex(10)]
        public byte MateType { get; set; }

        [PacketIndex(11)]
        public byte Unknow5 { get; set; }

        [PacketIndex(12)]
        public byte Unknow6 { get; set; }

        [PacketIndex(13)]
        public long SPSkill1 { get; set; }

        [PacketIndex(14)]
        public long SPSkill2 { get; set; }

        [PacketIndex(15)]
        public long SPSkill3 { get; set; }

        [PacketIndex(16)]
        public long SPRank1 { get; set; }

        [PacketIndex(17)]
        public long SPRank2 { get; set; }

        [PacketIndex(18)]
        public long SPRank3 { get; set; }

        #endregion Properties
    }
}