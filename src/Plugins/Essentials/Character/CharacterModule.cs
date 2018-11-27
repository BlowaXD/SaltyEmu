using System.Threading.Tasks;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Player.Extension;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.Character
{
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

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s HeroLevel has been changed to {level}."));
        }

        [Command("Reputation", "Reput", "rep")]
        [Description("Change the character's reputation.")]
        public Task<SaltyCommandResult> ChangeReputationAsync(
            [Description("Desired level.")] long reputation,
            [Description("Character you want to change the level")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeReputation(reputation);

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s Reputation has been changed to {reputation}."));
        }


        [Command("ChangeClass", "Class")]
        [Description("Change the character's herolevel.")]
        public Task<SaltyCommandResult> ChangeClassAsync(
            [Description("Class you want to change to")] CharacterClassType newClass,
            [Description("Character you want to change the level")] IPlayerEntity player = null)
        {
            if (player == null)
            {
                player = Context.Player;
            }

            player.ChangeClass(newClass);

            return Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name}'s class has been changed to {newClass.ToString()}."));
        }
    }
}