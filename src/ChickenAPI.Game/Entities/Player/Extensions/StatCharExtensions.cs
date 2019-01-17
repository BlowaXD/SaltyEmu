using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class StatCharExtensions
    {
        public static Task ActualizeUiStatChar(this IPlayerEntity player)
        {
            return player.SendPacketAsync(player.GenerateStatCharPacket());
        }

        public static ScPacket GenerateStatCharPacket(this IPlayerEntity player)
        {
            byte type = 0;
            byte subType = 0;

            switch (player.Character.Class)
            {
                case CharacterClassType.Adventurer:
                    type = 0;
                    subType = 1;
                    break;

                case CharacterClassType.Magician:
                    type = 2;
                    subType = 1;
                    break;

                case CharacterClassType.Swordman:
                    type = 0;
                    subType = 1;
                    break;

                case CharacterClassType.Archer:
                    type = 1;
                    subType = 0;
                    break;
            }

            return new ScPacket
            {
                Type = type,
                Type2 = subType,
                MainWeaponUpgrade = player.Weapon?.Upgrade ?? 0,
                MinHit = player.MinHit,
                MaxHit = player.MaxHit,
                HitRate = player.HitRate,
                CriticalHitRate = player.CriticalRate,
                CriticalHitMultiplier = player.CriticalChance,
                SecondaryWeaponUpgrade = player.SecondaryWeapon?.Upgrade ?? 0,
                SecondaryMinHit = player.SecondaryWeapon?.DamageMinimum ?? 0,
                SecondaryMaxHit = player.SecondaryWeapon?.DamageMaximum ?? 0,
                SecondaryHitRate = player.SecondaryWeapon?.HitRate ?? 0,
                SecondaryCriticalHitRate = player.SecondaryWeapon?.CriticalRate ?? 0,
                SecondaryCriticalHitMultiplier = player.SecondaryWeapon?.CriticalDamageRate ?? 0,
                ArmorUpgrade = player.Armor?.Upgrade ?? 0,
                Defence = player.Defence,
                DefenceRate = player.DefenceDodge,
                DistanceDefence = player.DistanceDefence,
                DistanceDefenceRate = player.DistanceDefenceDodge,
                MagicalDefence = player.MagicalDefence,
                FireResistance = player.FireResistance,
                WaterResistance = player.WaterResistance,
                LightResistance = player.LightResistance,
                DarkResistance = player.DarkResistance
            };
        }
    }
}