using System.Collections.Generic;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Group
{
    [PacketHeader("pidx")]
    public class PidxPacket : PacketBase
    {
        [PacketIndex(0)]
        public long GroupId { get; set; }

        [PacketIndex(1, SeparatorNestedElements = " ")]
        public List<PidxSubPacket> SubPackets { get; set; }

        [PacketHeader("pidx_subpacket")]
        public class PidxSubPacket : PacketBase
        {
            [PacketIndex(0)]
            public bool IsMemberOfGroup { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = ".")]
            public long CharacterId { get; set; }
        }
    }
}