using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.ServerPackets.Player;

namespace ChickenAPI.Game.Extensions.PacketGeneration
{
    public static class TcInfoPacketExtension
    {
        public static TitPacket GenerateTitPacket(this IPlayerEntity player)
        {
            return new TitPacket
            {
                ClassType = player.Character.Class == CharacterClassType.Adventurer ? "Adventurer" :
                    player.Character.Class == CharacterClassType.Archer ? "Archer" :
                    player.Character.Class == CharacterClassType.Magician ? "Mage" :
                    player.Character.Class == CharacterClassType.Swordman ? "Swordman" : "Martial",
                Name = player.Character.Name
            };
        }

        public static TcInfoPacket GenerateTcInfo(this IPlayerEntity charac)
        {
            ItemInstanceDto fairy = null;
            ItemInstanceDto armor = null;
            ItemInstanceDto weapon2 = null;
            ItemInstanceDto weapon = null;
            if (charac.Inventory != null)
            {
                fairy = charac.Inventory.GetWeared(EquipmentType.Fairy);
                armor = charac.Inventory.GetWeared(EquipmentType.Armor);
                weapon2 = charac.Inventory.GetWeared(EquipmentType.SecondaryWeapon);
                weapon = charac.Inventory.GetWeared(EquipmentType.MainWeapon);
            }

            bool isPvpPrimary = false;
            bool isPvpSecondary = false;
            bool isPvpArmor = false;

            if (weapon?.Item.Name.Contains(": ") == true)
            {
                isPvpPrimary = true;
            }

            isPvpSecondary |= weapon2?.Item.Name.Contains(": ") == true;
            isPvpArmor |= armor?.Item.Name.Contains(": ") == true;

            // tc_info 0 name 0 0 0 0 -1 - 0 0 0 0 0 0 0 0 0 0 0 wins deaths reput 0 0 0 morph
            // talentwin talentlose capitul rankingpoints arenapoints 0 0 ispvpprimary ispvpsecondary
            // ispvparmor herolvl desc
            return new TcInfoPacket
            {
                Level = charac.Level,
                Name = charac.Character.Name,
                Element = fairy?.ElementType ?? ElementType.Neutral,
                ElementRate = fairy?.ElementRate ?? 0,
                Class = charac.Character.Class,
                Gender = charac.Character.Gender,
                Family = charac.HasFamily ? charac.Family.Name : "-1 -",
                ReputationIco = charac.GetReputIcon(),
                DignityIco = charac.GetDignityIcon(),
                HaveWeapon = weapon != null ? 1 : 0,
                WeaponRare = weapon?.Rarity ?? 0,
                WeaponUpgrade = weapon?.Upgrade ?? 0,
                HaveSecondary = weapon2 != null ? 1 : 0,
                SecondaryRare = weapon2?.Rarity ?? 0,
                SecondaryUpgrade = weapon?.Upgrade ?? 0,
                HaveArmor = armor != null ? 1 : 0,
                ArmorRare = armor?.Rarity ?? 0,
                ArmorUpgrade = armor?.Upgrade ?? 0,
                Act4Kill = charac.Character.Act4Kill,
                Act4Dead = charac.Character.Act4Dead,
                Reputation = charac.Character.Reput,
                Unknow = 0,
                Unknow2 = 0,
                Unknow3 = 0,
                Unknow4 = 0,
                Morph = charac.MorphId,
                TalentLose = charac.Character.TalentLose,
                TalentSurrender = charac.Character.TalentSurrender,
                TalentWin = charac.Character.TalentWin,
                MasterPoints = charac.Character.MasterPoints,
                Compliments = charac.Character.Compliment,
                Act4Points = charac.Character.Act4Points,
                isPvpArmor = isPvpArmor,
                isPvpPrimary = isPvpPrimary,
                isPvpSecondary = isPvpSecondary,
                HeroLevel = charac.HeroLevel,
                Biography = string.IsNullOrEmpty(charac.Character.Biography) ? " REKT BOY" : charac.Character.Biography
            };
        }
    }
}