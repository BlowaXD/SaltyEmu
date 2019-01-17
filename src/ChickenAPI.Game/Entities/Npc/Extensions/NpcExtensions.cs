using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Packets.Game.Client.UI;

namespace ChickenAPI.Game.Entities.Npc.Extensions
{
    public static class NpcExtensions
    {
        public static EInfoPacket GenerateEInfoPacket(this NpcMonsterDto monster)
        {
            return new EInfoPacket
            {
                Unknown1 = 10,
                MonsterVnum = monster.Id,
                Level = monster.Level,
                Element = monster.Element,
                AttackClass = monster.AttackClass,
                ElementRate = monster.ElementRate,
                AttackUpgrade = monster.AttackUpgrade,
                DamageMinimum = monster.DamageMinimum,
                DamageMaximum = monster.DamageMaximum,
                Concentrate = monster.Concentrate,
                CriticalChance = monster.CriticalChance,
                CriticalRate = monster.CriticalRate,
                DefenceUpgrade = monster.DefenceUpgrade,
                CloseDefence = monster.CloseDefence,
                DefenceDodge = monster.DefenceDodge,
                DistanceDefence = monster.DistanceDefence,
                DistanceDefenceDodge = monster.DistanceDefenceDodge,
                MagicDefence = monster.MagicDefence,
                FireResistance = monster.FireResistance,
                WaterResistance = monster.WaterResistance,
                LightResistance = monster.LightResistance,
                DarkResistance = monster.DarkResistance,
                MaxHp = monster.MaxHp,
                MaxMp = monster.MaxMp,
                Unknown2 = -1,
                Name = monster.Name.Replace(" ", "^")
            };
        }
    }
}
