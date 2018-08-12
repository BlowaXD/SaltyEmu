using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Battle;

namespace ChickenAPI.Game.Packets.Game.Server
{
    [PacketHeader("st")]
    public class StPacket : PacketBase
    {
        public StPacket(IEntity entity)
        {
            BattleComponent battle = null;
            switch (entity)
            {
                case INpcEntity npc:
                    VisualType = VisualType.Npc;
                    VisualId = npc.MapNpc.Id;
                    Level = npc.MapNpc.NpcMonster.Level;
                    HeroLevel = npc.MapNpc.NpcMonster.HeroLevel;
                    battle = npc.Battle;
                    break;

                case IMonsterEntity monster:
                    VisualType = VisualType.Npc;
                    VisualId = monster.MapMonster.Id;
                    Level = monster.MapMonster.NpcMonster.Level;
                    HeroLevel = monster.MapMonster.NpcMonster.HeroLevel;
                    battle = monster.Battle;
                    break;

                case IPlayerEntity player:
                    VisualType = VisualType.Character;
                    VisualId = player.Character.Id;
                    Level = player.Experience.Level;
                    HeroLevel = player.Experience.HeroLevel;
                    battle = player.Battle;
                    break;
            }

            CardIds = null;

            if (battle == null)
            {
                return;
            }

            HpPercentage = battle.HpPercentage;
            MpPercentage = battle.MpPercentage;
            Hp = battle.Hp;
            Mp = battle.Mp;
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