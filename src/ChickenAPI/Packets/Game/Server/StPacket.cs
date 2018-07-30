using System.Collections.Generic;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("st")]
    public class StPacket : PacketBase
    {
        public StPacket(IEntity entity)
        {
            var battle = entity.GetComponent<BattleComponent>();
            var xp = entity.GetComponent<ExperienceComponent>();

            switch (entity.Type)
            {
                case EntityType.Player:
                    VisualType = VisualType.Character;
                    VisualId = entity.GetComponent<CharacterComponent>().Id;
                    break;
                case EntityType.Npc:
                case EntityType.Monster:
                    VisualType = entity.Type == EntityType.Monster ? VisualType.Monster : VisualType.Npc;
                    VisualId = entity.GetComponent<NpcMonsterComponent>().MapNpcMonsterId;
                    break;
            }

            Level = xp.Level;
            HeroLevel = xp.HeroLevel;
            HpPercentage = battle.HpPercentage;
            MpPercentage = battle.MpPercentage;
            Hp = battle.Hp;
            Mp = battle.Mp;
            CardIds = null;
        }


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