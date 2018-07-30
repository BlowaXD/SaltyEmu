namespace ChickenAPI.Packets.Game.Client
{
    [PacketHeader("drop")]
    class DropPacket : PacketBase
    {
        [PacketIndex(0)]
        public long ItemVNum { get; set; }

        [PacketIndex(1)]
        public long DropId { get; set; }

        [PacketIndex(2)]
        public short MapX { get; set; }

        [PacketIndex(3)]
        public short MapY { get; set; }

        [PacketIndex(4)]
        public short Amount { get; set; }

        [PacketIndex(5)]
        public bool IsQuestItem { get; set; }

        [PacketIndex(6)]
        public byte Unknown { get; set; } // looks like always 0

        [PacketIndex(7)]
        public byte Unknown2 { get; set; } // looks like always -1
    }
}
