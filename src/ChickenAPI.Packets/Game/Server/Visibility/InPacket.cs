using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Visibility
{
    [PacketHeader("in")]
    public class InPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1, IsOptional = true)]
        public string Name { get; set; }

        [PacketIndex(2)]
        public string TransportId { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public long? VNum { get; set; }

        [PacketIndex(4)]
        public short PositionX { get; set; }

        [PacketIndex(5)]
        public short PositionY { get; set; }

        [PacketIndex(6, IsOptional = true)]
        public DirectionType DirectionType { get; set; }

        [PacketIndex(7, IsOptional = true)]
        public long? Amount { get; set; }

        [PacketIndex(8, IsOptional = true, RemoveSeparator = true)]
        public InMonsterSubPacket InMonsterSubPacket { get; set; }

        [PacketIndex(9, IsOptional = true, RemoveSeparator = true)]
        public InCharacterSubPacketBase InCharacterSubPacket { get; set; }

        [PacketIndex(10, IsOptional = true, RemoveSeparator = true)]
        public InNpcSubPacket InNpcSubPacket { get; set; }

        [PacketIndex(10, IsOptional = true, RemoveSeparator = true)]
        public InItemSubPacketBase InDropSubPacket { get; set; }

        [PacketIndex(10, IsOptional = true, RemoveSeparator = true)]
        public InMateSubPacket InMateSubPacket { get; set; }

        #endregion Properties
    }
}