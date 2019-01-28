using System.Collections.Generic;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Group
{
    [PacketHeader("pinit")]
    public class PInitPacket : PacketBase
    {
        [PacketIndex(0)]
        public long PartySize { get; set; }

        [PacketIndex(1, SeparatorBeforeProperty = "", SeparatorNestedElements = " ", IsOptional = true)]
        public List<PInitMateSubPacket> MateSubPackets { get; set; }

        [PacketIndex(2, SeparatorBeforeProperty = "", SeparatorNestedElements = " ", IsOptional = true)]
        public List<PInitPlayerSubPacket> PlayerSubPackets { get; set; }

        [PacketHeader("pinit_subpacket")]
        public class PInitMateSubPacket : PacketBase
        {
            [PacketIndex(0)]
            public VisualType VisualType { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = "|")]
            public long VisualId { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = "|")]
            public byte GroupIndex { get; set; }

            [PacketIndex(3, SeparatorBeforeProperty = "|")]
            public long Level { get; set; }

            [PacketIndex(4, SeparatorBeforeProperty = "|")]
            public string Name { get; set; }

            [PacketIndex(5, SeparatorBeforeProperty = "|")]
            public short Unknown { get; set; }

            [PacketIndex(6, SeparatorBeforeProperty = "|")]
            public long MorphOrNpcMonsterId { get; set; }

            [PacketIndex(7, SeparatorBeforeProperty = "|")]
            public short Unknown2 { get; set; }
        }

        [PacketHeader("pinit_subpacket")]
        public class PInitPlayerSubPacket : PacketBase
        {
            [PacketIndex(0)]
            public VisualType VisualType { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = "|")]
            public long VisualId { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = "|")]
            public byte GroupIndex { get; set; }

            [PacketIndex(3, SeparatorBeforeProperty = "|")]
            public long Level { get; set; }

            [PacketIndex(4, SeparatorBeforeProperty = "|")]
            public string Name { get; set; }

            [PacketIndex(5, SeparatorBeforeProperty = "|")]
            public byte Unknown { get; set; }

            [PacketIndex(6, SeparatorBeforeProperty = "|")]
            public GenderType Gender { get; set; }

            [PacketIndex(7, SeparatorBeforeProperty = "|")]
            public CharacterClassType Class { get; set; }

            [PacketIndex(8, SeparatorBeforeProperty = "|")]
            public long MorphId { get; set; }

            [PacketIndex(9, SeparatorBeforeProperty = "|")]
            public long HeroLevel { get; set; }
        }
    }
}