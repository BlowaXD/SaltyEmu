using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StPacketExtensions
    {
        public static StPacket GenerateStPacket(this IEntity entity)
        {
            var stPacket = new StPacket();
            switch (entity)
            {
                case IPlayerEntity player:
                    stPacket.VisualType = VisualType.Character;
                    stPacket.VisualId = player.Character.Id;
                    stPacket.Level = player.Level;
                    stPacket.HeroLevel = player.HeroLevel;
                    stPacket.HpPercentage = player.HpPercentage;
                    stPacket.MpPercentage = player.MpPercentage;
                    stPacket.Hp = player.Hp;
                    stPacket.Mp = player.Mp;
                    break;
                case INpcEntity npc:
                    stPacket.VisualType = VisualType.Npc;
                    stPacket.VisualId = npc.MapNpc.Id;
                    stPacket.Level = npc.MapNpc.NpcMonster.Level;
                    stPacket.HeroLevel = npc.MapNpc.NpcMonster.HeroLevel;
                    stPacket.HpPercentage = npc.HpPercentage;
                    stPacket.MpPercentage = npc.MpPercentage;
                    stPacket.Hp = npc.Hp;
                    stPacket.Mp = npc.Mp;
                    break;
                case IMonsterEntity monster:

                    stPacket.VisualType = VisualType.Monster;
                    stPacket.VisualId = monster.MapMonster.Id;
                    stPacket.Level = monster.MapMonster.NpcMonster.Level;
                    stPacket.HeroLevel = monster.MapMonster.NpcMonster.HeroLevel;
                    stPacket.HpPercentage = monster.HpPercentage;
                    stPacket.MpPercentage = monster.MpPercentage;
                    stPacket.Hp = monster.Hp;
                    stPacket.Mp = monster.Mp;
                    break;
            }

            stPacket.CardIds = null;
            return stPacket;
        }
    }
}