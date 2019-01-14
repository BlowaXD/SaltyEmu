using System.Collections.Generic;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Relations
{
    [PacketHeader("blinit")]
    public class BlInitPacket : PacketBase
    {
        [PacketIndex(0, SeparatorNestedElements = " ")]
        public List<BlInitSubPacket> SubPackets { get; set; }

        [PacketHeader("blinit_subpacket")]
        public class BlInitSubPacket
        {
            [PacketIndex(0)]
            public long CharacterId { get; set; }

            [PacketIndex(1)]
            public string CharacterName { get; set; }
        }
    }
}