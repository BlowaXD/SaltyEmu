using System.Collections.Generic;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Entities
{
    [PacketHeader("st")]
    public class StPacket : PacketBase
    {
        //st 1 {CharacterId} {Level} {HeroLevel} {(int)(Hp / (float)HpLoad() * 100)} {(int)(Mp / (float)MpLoad() * 100)} {Hp} {Mp}{Buff.Aggregate(string.Empty, (current, buff) => current + $" {buff.Card.CardId}")}
        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long VisualId { get; set; }


        [PacketIndex(2)]
        public byte Level { get; set; }


        [PacketIndex(3)]
        public byte HeroLevel { get; set; }


        [PacketIndex(4)]
        public long HpPercentage { get; set; }


        [PacketIndex(5)]
        public long MpPercentage { get; set; }


        [PacketIndex(6)]
        public long Hp { get; set; }

        [PacketIndex(7)]
        public long Mp { get; set; }

        [PacketIndex(8, IsOptional = true, RemoveSeparator = true)]
        public List<long> CardIds { get; set; }
    }
}