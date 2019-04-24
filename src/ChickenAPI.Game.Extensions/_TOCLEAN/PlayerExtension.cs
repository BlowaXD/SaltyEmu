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
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Shops.Extensions;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game._i18n;
using ChickenAPI.Game.Extensions.PacketGeneration;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Player;
using ChickenAPI.Packets.ServerPackets.UI;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerExtension
    {
        private static readonly IRandomGenerator _randomGenerator = new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;
        private static readonly IAlgorithmService Algorithm = new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;


        public static async Task ChangeClass(this IPlayerEntity player, CharacterClassType type)
        {
            player.JobLevel = 1;
            player.JobLevelXp = 0;
            await player.SendPacketAsync(new NpInfoPacket { Page = 0 });
            await player.SendPacketAsync(new PclearPacket());

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

            foreach (SkillDto skill in player.Skills.Values)
            {
                if (skill.Id >= 200)
                {
                    player.Skills.Remove(skill.Id);
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
            await player.SendMessageAsync(PlayerMessages.UPGRADE_RARIFY_SUCCESS, MessageType.White);
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