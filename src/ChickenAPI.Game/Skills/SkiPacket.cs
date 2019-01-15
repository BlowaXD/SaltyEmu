using System.Collections.Generic;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Game.Skills
{
    [PacketHeader("ski")]
    public class SkiPacket : PacketBase
    {
        [PacketIndex(0, SeparatorNestedElements = " ")]
        public List<long> SkillIds { get; set; }
    }
}