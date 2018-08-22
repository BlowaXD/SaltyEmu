using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class StatCharExtensions
    {
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
                MainWeaponUpgrade = player.Inventory.Wear[(int)EquipmentType.MainWeapon]?.Upgrade ?? 0,
                MinHit = 0,
                MaxHit = 0,
                HitRate = 0,
                CriticalHitRate = 0,
                CriticalHitMultiplier = 0,
                Type2 = subType,
                SecondaryWeaponUpgrade = player.Inventory.Wear[(int)EquipmentType.MainWeapon]?.Upgrade ?? 0,
                SecondaryMinHit = 0,
                SecondaryMaxHit = 0,
                SecondaryHitRate = 0,
                SecondaryCriticalHitRate = 0,
                SecondaryCriticalHitMultiplier = 0,
                ArmorUpgrade = player.Inventory.Wear[(int)EquipmentType.Armor]?.Upgrade ?? 0,
                Defence = 0,
                DefenceRate = 0,
                DistanceDefence = 0,
                DistanceDefenceRate = 0,
                MagicalDefence = 0,
                FireResistance = 0,
                WaterResistance = 0,
                LightResistance = 0,
                DarkResistance = 0
            };
        }
    }
}