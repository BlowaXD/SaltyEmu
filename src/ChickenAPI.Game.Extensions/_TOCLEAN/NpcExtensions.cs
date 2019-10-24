using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Packets.ServerPackets.Inventory;

namespace ChickenAPI.Game.Entities.Npc.Extensions
{
    public static class NpcExtensions
    {
        public static EInfoPacket GenerateEInfoPacket(this NpcMonsterDto monster)
        {
            return new EInfoPacket
            {/*
                Unknown1 = 10,
                ItemVNum = monster.Id,
                LevelMinimum = monster.Level,
                Element = (byte?)monster.Element,
                AttackClass = monster.AttackClass,
                ElementRate = monster.ElementRate,
                Upgrade = monster.AttackUpgrade,
                
                DamageMinimum = monster.DamageMinimum,
                DamageMaximum = monster.DamageMaximum,
                Concentrate = monster.Concentrate,
                CriticalChance = monster.CriticalChance,
                CriticalRate = monster.CriticalRate,
                DefenceUpgrade = monster.DefenceUpgrade,
                CloseDefense = monster.CloseDefence,
                DefenseDodge = monster.DefenceDodge,
                RangeDefense =  monster.DistanceDefence,
                DistanceDefenceDodge = monster.DistanceDefenceDodge,
                MagicDefense = monster.MagicDefence,
                
                FireResistance = monster.FireResistance,
                WaterResistance = monster.WaterResistance,
                LightResistance = monster.LightResistance,
                DarkResistance = monster.DarkResistance,
                MaxHp = monster.MaxHp,
                MaxMp = monster.MaxMp,
                GroupId = -1,
                Name = monster.Name.Replace(" ", "^")
                */
            };
        }
    }
}
