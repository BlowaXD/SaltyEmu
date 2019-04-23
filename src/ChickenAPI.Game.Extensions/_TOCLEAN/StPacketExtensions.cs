using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Packets.ServerPackets.Entities;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class StPacketExtensions
    {
        public static StPacket GenerateStPacket(this IBattleEntity battle) =>
            new StPacket
            {
                Type = battle.Type,
                VisualId = battle.Id,
                Level = battle.Level,
                HeroLvl = battle.HeroLevel,
                HpPercentage = battle.HpPercentage,
                MpPercentage = battle.MpPercentage,
                CurrentHp = battle.Hp,
                CurrentMp = battle.Mp,
                BuffIds = new List<short>(battle.Buffs.Select(s => (short)s.Id))
            };
    }
}