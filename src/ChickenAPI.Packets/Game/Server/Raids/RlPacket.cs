using System.Collections.Generic;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Raids
{
    [PacketHeader("rl")]
    public class RlPacket
    {
        [PacketIndex(0)]
        public RlPacketType Type { get; set; }

        [PacketIndex(1, SeparatorNestedElements = " ")]
        public List<RaidListPacketDetail> Raids { get; set; }

        [PacketHeader("rl_packet_details")]
        public class RaidListPacketDetail : PacketBase
        {
            [PacketIndex(0)]
            public long RaidId { get; set; }

            [PacketIndex(1, SeparatorBeforeProperty = ".")]
            public long LevelMinimum { get; set; }

            [PacketIndex(2, SeparatorBeforeProperty = ".")]
            public long LevelMaximum { get; set; }

            [PacketIndex(3, SeparatorBeforeProperty = ".")]
            public string LeaderName { get; set; }

            [PacketIndex(4, SeparatorBeforeProperty = ".")]
            public long LeaderLevel { get; set; }

            [PacketIndex(5, SeparatorBeforeProperty = ".")]
            public long LeaderMorphId { get; set; }

            [PacketIndex(6, SeparatorBeforeProperty = ".")]
            public CharacterClassType LeaderClass { get; set; }

            [PacketIndex(7, SeparatorBeforeProperty = ".")]
            public GenderType LeaderGender { get; set; }

            [PacketIndex(8, SeparatorBeforeProperty = ".")]
            public long PlayerCountInGroup { get; set; }

            [PacketIndex(9, SeparatorBeforeProperty = ".")]
            public long LeaderHeroLevel { get; set; }
        }
    }
}