using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Battle;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StPacketExtensions
    {
        public static StPacket GenerateStPacket(this IEntity entity)
        {
            var stPacket = new StPacket();
            BattleComponent battle = null;
            switch (entity)
            {
                case IPlayerEntity player:
                    stPacket.VisualType = VisualType.Character;
                    stPacket.VisualId = player.Character.Id;
                    stPacket.Level = player.Experience.Level;
                    stPacket.HeroLevel = player.Experience.HeroLevel;
                    battle = player.Battle;
                    break;
                case INpcEntity npc:
                    stPacket.VisualType = VisualType.Npc;
                    stPacket.VisualId = npc.MapNpc.Id;
                    stPacket.Level = npc.MapNpc.NpcMonster.Level;
                    stPacket.HeroLevel = npc.MapNpc.NpcMonster.HeroLevel;
                    battle = npc.Battle;
                    break;
                case IMonsterEntity monster:

                    stPacket.VisualType = VisualType.Monster;
                    stPacket.VisualId = monster.MapMonster.Id;
                    stPacket.Level = monster.MapMonster.NpcMonster.Level;
                    stPacket.HeroLevel = monster.MapMonster.NpcMonster.HeroLevel;
                    battle = monster.Battle;
                    break;
            }

            stPacket.CardIds = null;

            if (battle == null)
            {
                return stPacket;
            }

            stPacket.HpPercentage = battle.HpPercentage;
            stPacket.MpPercentage = battle.MpPercentage;
            stPacket.Hp = battle.Hp;
            stPacket.Mp = battle.Mp;
            return stPacket;
        }
    }
}