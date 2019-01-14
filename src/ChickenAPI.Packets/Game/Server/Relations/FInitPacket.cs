using System.Collections.Generic;
using ChickenAPI.Enums.Game.Relations;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Relations
{
    [PacketHeader("finit")]
    public class FInitPacket : PacketBase
    {
        [PacketIndex(0, SeparatorNestedElements = " ")]
        public List<FInitSubPacket> Packets { get; set; }

        [PacketHeader("finit_subpacket")]
        public class FInitSubPacket : PacketBase
        {
            [PacketIndex(0)]
            public long RelationId { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = "|")]
            public CharacterRelationType RelationType { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = "|")]
            public bool IsOnline { get; set; }

            [PacketIndex(3, SeparatorBeforeProperty = "|")]
            public string CharacterName { get; set; }
        }
    }
}