using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Group
{
    [PacketHeader("pst")]
    public class PstPacket : PacketBase
    {
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }

        [PacketIndex(2)]
        public long GroupIndex { get; set; }

        [PacketIndex(3)]
        public int HpPercentage { get; set; }

        [PacketIndex(4)]
        public int MpPercentage { get; set; }

        [PacketIndex(5)]
        public int HpMax { get; set; }

        [PacketIndex(6)]
        public int MpMax { get; set; }

        [PacketIndex(7)]
        public GenderType Gender { get; set; }

        [PacketIndex(8)]
        public CharacterClassType Class { get; set; }

        [PacketIndex(9)]
        public int Morph { get; set; }

        [PacketIndex(10)]
        public string Buffs { get; set; }
    }
}