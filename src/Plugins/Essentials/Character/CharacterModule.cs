using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
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
        [Command("Level", "lev", "lvl")]
        [Description("Change the character level.")]
        public Task<SaltyCommandResult> ChangeLevelAsync(
            [Description("Desired level.")] byte level,
            [Description("Character you want to change the level")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeLevel(level);
            player.Broadcast(player.GenerateLevelUpPacket());

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s level has been changed to {level}."));
        }

        [Command("JobLevel", "JLevel", "jlev", "jlvl")]
        [Description("Change the character level.")]
        public Task<SaltyCommandResult> ChangeJobLevelAsync(
            [Description("Desired level.")] byte level,
            [Description("Character you want to change the level")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeJobLevel(level);
            player.Broadcast(player.GenerateLevelUpPacket());

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s JobLevel has been changed to {level}."));
        }

        [Command("HeroLevel", "HLevel", "hlev", "hlvl")]
        [Description("Change the character's herolevel.")]
        public Task<SaltyCommandResult> ChangeHeroLevelAsync(
            [Description("Desired level.")] byte level,
            [Description("Character you want to change the level")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeHeroLevel(level);
            player.Broadcast(player.GenerateLevelUpPacket());

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s HeroLevel has been changed to {level}."));
        }

        [Command("FairyLevel", "FLevel", "flev", "flvl")]
        [Description("Change the character's fairy's level.")]
        public Task<SaltyCommandResult> ChangeFairyLevelAsync(
            [Description("Desired level.")] short level,
            [Description("Character you want to change the level")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            ItemInstanceDto fairy = player.Inventory.GetWeared(EquipmentType.Fairy);
            if (fairy == null)
            {
                return Task.FromResult(new SaltyCommandResult(false, $"{player.Character.Name} has no Fairy actually weared"));
            }

            fairy.ElementRate = level;
            // stats infos

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s FairyLevel has been changed to {level}."));
        }

        [Command("Reputation", "Reput", "rep")]
        [Description("Change the character's reputation.")]
        public Task<SaltyCommandResult> ChangeReputationAsync(
            [Description("Desired reputation.")] long reputation,
            [Description("Character you want to change the reputation")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeReputation(reputation);

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s Reputation has been changed to {reputation}."));
        }


        [Command("ChangeClass", "Class")]
        [Description("Change the character's class.")]
        public Task<SaltyCommandResult> ChangeClassAsync(
            [Description("Class you want to change to")] CharacterClassType newClass,
            [Description("Character you want to change the class")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeClass(newClass);

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s class has been changed to {newClass.ToString()}."));
        }

        [Command("AddGold")]
        [Description("Add the given amount of gold to an inventory.")]
        public Task<SaltyCommandResult> AddGoldAsync(
            [Description("Desired amount to add.")] long count,
            [Description("Player you want to add gold.")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.GoldUp(count);

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s gold has been increased by {count}"));
        }

        [Command("SetGold", "Gold")]
        [Description("Set the given amount of gold to an inventory.")]
        public Task<SaltyCommandResult> SetGoldAsync(
            [Description("Desired amount to set.")] long count,
            [Description("Player you want to set gold.")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Character.Gold = count;
            player.ActualizeUiGold();

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s gold has been set to {count}"));
        }

        [Command("Where")]
        [Description("Provide the player's coordinates [x] [y] and name of the map in the chat.")]
        public Task<SaltyCommandResult> WhereAsync(
            [Description("Player you want to locate")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s position: map {player.CurrentMap.Map.Id} [{player.Position.X}, {player.Position.Y}]"));
        }

        [Command("Speed")]
        [Description("Change the speed of the character.")]
        public Task<SaltyCommandResult> SpeedAsync(
            [Description("Desired speed.")] byte speed,
            [Description("Player you want to change the speed")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Speed = speed > 59 ? (byte)59 : speed;
            player.ActualizePlayerCondition();

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s speed is now {player.Speed}"));
        }

        [Command("SpeedReset", "rspeed", "resetspeed")]
        [Description("Reset the speed of the character.")]
        public Task<SaltyCommandResult> SpeedAsync([Description("Player you want to reset the speed")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.Speed = (byte)ChickenContainer.Instance.Resolve<IAlgorithmService>().GetSpeed(player.Character.Class, player.Level);
            player.ActualizePlayerCondition();

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s speed is now {player.Speed}"));
        }
    }
}