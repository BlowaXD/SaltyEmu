using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StPacketExtensions
    {
        public static StPacket GenerateStPacket(this IBattleEntity battle) =>
            new StPacket
            {
                VisualType = battle.Type,
                VisualId = battle.Id,
                Level = battle.Level,
                HeroLevel = battle.HeroLevel,
                HpPercentage = battle.HpPercentage,
                MpPercentage = battle.MpPercentage,
                Hp = battle.Hp,
                Mp = battle.Mp,
                CardIds = new List<long>(battle.Buffs.Select(s => s.Id))
            };
    }
}