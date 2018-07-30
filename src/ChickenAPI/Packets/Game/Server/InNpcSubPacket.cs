namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("in_npc_subpacket")]
    public class InNpcSubPacket : PacketBase
    {
        [PacketIndex(0)]
        public byte HpPercentage { get; set; }

        [PacketIndex(1)]
        public byte MpPercentage { get; set; }

        [PacketIndex(2)]
        public int Dialog { get; set; }

        [PacketIndex(3)]
        public int Unknown1 { get; set; }

        [PacketIndex(4)]
        public int Unknown2 { get; set; }

        [PacketIndex(5)]
        public int Unknown3 { get; set; }

        [PacketIndex(6)]
        public int Unknown4 { get; set; }

        [PacketIndex(7)]
        public bool IsSitting { get; set; }

        [PacketIndex(8)]
        public int Unknown5 { get; set; }

        [PacketIndex(9)]
        public string Unknown6 { get; set; }

        [PacketIndex(10)]
        public int Unknown7 { get; set; }

        [PacketIndex(11)]
        public int Unknown8 { get; set; }

        [PacketIndex(12)]
        public int Unknown9 { get; set; }

        [PacketIndex(13)]
        public int Unknown10 { get; set; }

        [PacketIndex(14)]
        public int Unknown11 { get; set; }

        [PacketIndex(15)]
        public int Unknown12 { get; set; }

        [PacketIndex(16)]
        public int Unknown13 { get; set; }

        [PacketIndex(17)]
        public int Unknown14 { get; set; }

        [PacketIndex(18)]
        public int Unknown15 { get; set; }

        [PacketIndex(19)]
        public int Unknown16 { get; set; }
    }
}