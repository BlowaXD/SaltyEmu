using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets.Game.Client.Player;
using ChickenAPI.Packets.Game.Server.Player;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerExtension
    {
        private static readonly IRandomGenerator _randomGenerator = new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;
        private static readonly IAlgorithmService Algorithm = new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;

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

        public static async Task ChangeClass(this IPlayerEntity player, CharacterClassType type)
        {
            player.JobLevel = 1;
            player.JobLevelXp = 0;
            await player.SendPacketAsync(new NpInfoPacket { UnKnow = 0 });
            await player.SendPacketAsync(new PClearPacket());

            if (type == CharacterClassType.Adventurer)
            {
                player.Character.HairStyle = player.Character.HairStyle > HairStyleType.HairStyleB ? HairStyleType.HairStyleA : player.Character.HairStyle;
            }

            player.Character.Class = type;
            player.HpMax = Algorithm.GetHpMax(type, player.Level);
            player.MpMax = Algorithm.GetMpMax(type, player.Level);
            player.Hp = player.HpMax;
            player.Mp = player.MpMax;
            await player.SendPacketAsync(player.GenerateTitPacket());
            await player.ActualizeUiHpBar();
            await player.SendPacketAsync(player.GenerateEqPacket());
            await player.SendPacketAsync(player.GenerateEffectPacket(8));
            await player.SendPacketAsync(player.GenerateEffectPacket(196));

            await player.SendPacketAsync(player.GenerateScrPacket());
            await player.SendChatMessageFormat(PlayerMessages.CHARACTER_YOUR_CLASS_CHANGED_TO_X, SayColorType.Blue, type);

            player.Character.Faction = player.Family?.FamilyFaction ?? (FactionType)(1 + _randomGenerator.Next(0, 2));
            await player.SendChatMessageFormat(PlayerMessages.CHARACTER_YOUR_FACTION_CHANGED_TO_X, SayColorType.Blue, player.Character.Faction);

            await player.ActualizeUiFaction();
            await player.ActualizeUiStatChar();
            await player.SendPacketAsync(player.GenerateEffectPacket(4799 + (byte)player.Character.Faction));
            await player.ActualizePlayerCondition();
            await player.ActualizeUiExpBar();
            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.BroadcastAsync(player.GenerateInPacket());
            await player.BroadcastAsync(player.GenerateGidxPacket());
            await player.BroadcastAsync(player.GenerateEffectPacket(6));
            await player.BroadcastAsync(player.GenerateEffectPacket(196));

            SkillComponent component = player.SkillComponent;
            foreach (SkillDto skill in component.Skills.Values)
            {
                if (skill.Id >= 200)
                {
                    component.Skills.Remove(skill.Id);
                }
            }

            // TODO : LATER ADD SKILL
            await player.ActualizeUiSkillList();

            // Later too
            /* foreach (QuicklistEntryDTO quicklists in DAOFactory.QuicklistEntryDAO.LoadByCharacterId(CharacterId).Where(quicklists => QuicklistEntries.Any(qle => qle.Id == quicklists.Id)))
             {
                 DAOFactory.QuicklistEntryDAO.Delete(quicklists.Id);
             }

             QuicklistEntries = new List<QuicklistEntryDTO>
             {
                 new QuicklistEntryDTO
                 {
                     CharacterId = CharacterId,
                     Q1 = 0,
                     Q2 = 9,
                     Type = 1,
                     Slot = 3,
                     Pos = 1
                 }
             };
             if (ServerManager.Instance.Groups.Any(s => s.IsMemberOfGroup(Session) && s.GroupType == GroupType.Group))
             {
                 Session.CurrentMapInstance?.Broadcast(Session, $"pidx 1 1.{CharacterId}", ReceiverType.AllExceptMe);
             }*/
        }

        public static async Task ChangeGender(this IPlayerEntity player, GenderType type)
        {
            await player.SendChatMessageFormat(PlayerMessages.CHARACTER_X_GENDER_CHANGED_TO_Y, SayColorType.Blue, type);
            player.Character.Gender = type;
            await player.ActualizePlayerCondition();
            await player.SendPacketAsync(player.GenerateEqPacket());
            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.BroadcastAsync(player.GenerateInPacket());
            await player.BroadcastAsync(player.GenerateGidxPacket());
            await player.BroadcastAsync(player.GenerateEffectPacket(196));
        }


        public static async Task NotifyRarifyResult(this IPlayerEntity player, sbyte rare)
        {
            await player.SendMessageAsync(PlayerMessages.UPGRADE_RARIFY_SUCCESS, MsgPacketType.Whisper);
            await player.SendPacketAsync(player.GenerateSayPacket("RARIFY_SUCCESS " + rare, SayColorType.Green));
            await player.BroadcastAsync(player.GenerateEffectPacket(3005));
            await player.SendPacketAsync(player.GenerateShopEndPacket(ShopEndPacketType.CloseSubWindow));
        }

        #region Gold

        public static Task GoldLess(this IPlayerEntity player, long amount)
        {
            player.Character.Gold -= amount;
            return player.ActualizeUiGold();
        }

        public static Task GoldUp(this IPlayerEntity player, long amount)
        {
            player.Character.Gold += amount;
            return player.ActualizeUiGold();
        }

        #endregion Gold
    }
}