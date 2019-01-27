using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.Character
{
    [Name("Character")]
    [Description("Module related to character. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    internal sealed class CharacterModule : SaltyModuleBase
    {
        [Command("Kick")]
        [Description("Kick X user.")]
        public async Task<SaltyCommandResult> KickPlayerAsync(IPlayerEntity player = null, byte timer = 0, string reasons = null)
        {
            if (player == null)
            {
                return new SaltyCommandResult(false, "Please specify the charname to kick.");
            }

            if (reasons != null)
            {
                await player.SendModalAsync($"You will be kicked in {timer} seconds , for the following reasons: {reasons}", ModalPacketType.Default);
            }

            if (timer != 0)
            {
                await Task.Delay(timer * 1000);
            }

            player.Session.Disconnect();

            return new SaltyCommandResult(true, $"{player.Character.Name}'s has been Kicked.");
        }

        [Command("Level", "lev", "lvl")]
        [Description("Change the character level.")]
        public async Task<SaltyCommandResult> ChangeLevelAsync(
            [Description("Desired level.")] byte level,
            [Description("Character you want to change the level")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await player.ChangeLevel(level);
            await player.BroadcastAsync(player.GenerateLevelUpPacket());

            return new SaltyCommandResult(true, $"{player.Character.Name}'s level has been changed to {level}.");
        }

        [Command("JobLevel", "JLevel", "jlev", "jlvl")]
        [Description("Change the character level.")]
        public async Task<SaltyCommandResult> ChangeJobLevelAsync(
            [Description("Desired level.")] byte level,
            [Description("Character you want to change the level")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await player.ChangeJobLevel(level);
            await player.BroadcastAsync(player.GenerateLevelUpPacket());

            return new SaltyCommandResult(true, $"{player.Character.Name}'s Joblevel has been changed to {level}.");
        }

        [Command("HeroLevel", "HLevel", "hlev", "hlvl")]
        [Description("Change the character's herolevel.")]
        public async Task<SaltyCommandResult> ChangeHeroLevelAsync(
            [Description("Desired level.")] byte level,
            [Description("Character you want to change the level")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await player.ChangeHeroLevel(level);
            await player.BroadcastAsync(player.GenerateLevelUpPacket());

            return new SaltyCommandResult(true, $"{player.Character.Name}'s HeroLevel has been changed to {level}.");
        }

        [Command("FairyLevel", "FLevel", "flev", "flvl")]
        [Description("Change the character's fairy's level.")]
        public async Task<SaltyCommandResult> ChangeFairyLevelAsync(
            [Description("Desired level.")] short level,
            [Description("Character you want to change the level")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            ItemInstanceDto fairy = player.Inventory.GetWeared(EquipmentType.Fairy);
            if (fairy == null)
            {
                return new SaltyCommandResult(true, $"{player.Character.Name} has no Fairy actually weared");
            }

            fairy.ElementRate = level;
            await player.ActualizeUiStatChar();

            // stats infos
            return new SaltyCommandResult(true, $"{player.Character.Name}'s FairyLevel has been changed to {level}.");
        }

        [Command("Reputation", "Reput", "rep")]
        [Description("Change the character's reputation.")]
        public async Task<SaltyCommandResult> ChangeReputationAsync(
            [Description("Desired reputation.")] long reputation,
            [Description("Character you want to change the reputation")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Character.Reput = reputation;
            await player.ActualizeUiReputation();

            return new SaltyCommandResult(true, $"{player.Character.Name}'s Reputation has been changed to {reputation}.");
        }

        [Command("ChangeClass", "Class")]
        [Description("Change the character's class.")]
        public async Task<SaltyCommandResult> ChangeClassAsync(
            [Description("Class you want to change to")]
            CharacterClassType newClass,
            [Description("Character you want to change the class")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await player.ChangeClass(newClass);

            return new SaltyCommandResult(true, $"{player.Character.Name}'s class has been changed to {newClass.ToString()}.");
        }

        [Command("AddGold")]
        [Description("Add the given amount of gold to an inventory.")]
        public async Task<SaltyCommandResult> AddGoldAsync(
            [Description("Desired amount to add.")]
            long count,
            [Description("Player you want to add gold.")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await player.GoldUp(count);

            return new SaltyCommandResult(true, $"{player.Character.Name}'s gold has been increased by {count}");
        }

        [Command("SetGold", "Gold")]
        [Description("Set the given amount of gold to an inventory.")]
        public async Task<SaltyCommandResult> SetGoldAsync(
            [Description("Desired amount to set.")]
            long count,
            [Description("Player you want to set gold.")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Character.Gold = count;
            await player.ActualizeUiGold();

            return new SaltyCommandResult(true, $"{player.Character.Name}'s gold has been set to {count}");
        }

        [Command("Where")]
        [Description("Provide the player's coordinates [x] [y] and name of the map in the chat.")]
        public async Task<SaltyCommandResult> WhereAsync(
            [Description("Player you want to locate")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await Context.Player.SendChatMessageAsync("+---------------[Position]---------------+\n" +
                $"Nickname: {player.Character.Name}\n" +
                $"MapID: {player.CurrentMap.Map.Id}\n" +
                $"Coordinate - X: {player.Position.X} Y: {player.Position.Y}\n" +
                $"+-------------------------------------------+", SayColorType.Yellow);

            return new SaltyCommandResult(true, $"Position of {player.Character.Name} command has been sent.");
        }

        [Command("Speed")]
        [Description("Change the speed of the character.")]
        public async Task<SaltyCommandResult> SpeedAsync(
            [Description("Desired speed.")] byte speed,
            [Description("Player you want to change the speed")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Speed = speed > 59 ? (byte)59 : speed;
            await player.ActualizePlayerCondition();

            return new SaltyCommandResult(true, $"{player.Character.Name}'s speed is now {player.Speed}");
        }

        [Command("SpeedReset", "rspeed", "resetspeed")]
        [Description("Reset the speed of the character.")]
        public async Task<SaltyCommandResult> SpeedAsync(
            [Description("Player you want to reset the speed")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Speed = (byte)ChickenContainer.Instance.Resolve<IAlgorithmService>().GetSpeed(player.Character.Class, player.Level);
            await player.ActualizePlayerCondition();

            return new SaltyCommandResult(true, $"{player.Character.Name}'s speed is now {player.Speed}");
        }

        [Command("Info", "charInfo", "Information")]
        [Description("Display information about the player.")]
        public async Task<SaltyCommandResult> InfoAsync(
            [Description("Player you want to see information about him.")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await Context.Player.SendChatMessageAsync("+---------------[Information]---------------+", SayColorType.Green);
            await Context.Player.SendChatMessageAsync($"AccountID: {player.Character.AccountId}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"CharacterID: {player.Character.Id}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Nickname: {player.Character.Name}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Class: {player.Character.Class}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Level: {player.Character.Level} | {player.Character.LevelXp} XP", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Job: {player.Character.JobLevel} | {player.Character.JobLevelXp} XP", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"HeroLevel: {player.Character.HeroLevel} | {player.Character.HeroXp} XP", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"R Mode: {player.Character.RagePoint}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"HP: {player.Hp}/{player.HpMax}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"MP: {player.Mp}/{player.MpMax}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Speed: {player.Speed}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Gold: {player.Character.Gold}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Faction: {player.Character.Faction}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Family: {player.Family}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync($"Coordinate - MapID: {player.CurrentMap.Map.Id} X: {player.Position.X} Y: {player.Position.Y}", SayColorType.Yellow);
            await Context.Player.SendChatMessageAsync("+-----------------------------------------------+", SayColorType.Green);

            return new SaltyCommandResult(true, $"Information of {player.Character.Name} command has been sent.");
        }

        [Command("ChangeGender", "Gender")]
        [Description("Change the character's gender.")]
        public async Task<SaltyCommandResult> ChangeGenderAsync(
            [Description("Gender you want to change to")]
            GenderType newGender,
            [Description("Character you want to change the gender")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            await player.ChangeGender(newGender);

            return new SaltyCommandResult(true, $"{player.Character.Name}'s gender has been changed to {newGender.ToString()}.");
        }

        [Command("Heal")]
        [Description("Fully restore the character.")]
        public async Task<SaltyCommandResult> HealAsync(
            [Description("Player you want to heal")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Hp = player.HpMax;
            player.Mp = player.MpMax;
            await player.ActualizeUiHpBar();

            return new SaltyCommandResult(true, $"{player.Character.Name}'s HP & MP has been fully restored.");
        }

        public async Task<SaltyCommandResult> ChangeSyzeAsync(
            [Description("Size you want to ")] byte size,
            [Description("Player you want to change the size")]
            IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Size = size;
            await player.ActualizeUiSize();
            return new SaltyCommandResult(true, $"{player.Character.Name}'s size has been changed to {size}");
        }
    }
}