using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Managers;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.Teleport
{
    [Group("Teleport")]
    [Description("It's a module related to teleportation. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class TeleportModule : SaltyModuleBase
    {
        [Command("ToMe")]
        [Description("Command that teleports the given player to you")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> TeleportToMeAsync([Description("Name of the character you want to teleport")]
            string target,
            [Description("Amount of milliseconds to wait before teleporting him to you, by default no delay")]
            int delay = 0)
        {
            var manager = ChickenContainer.Instance.Resolve<IPlayerManager>();
            IPlayerEntity player = manager.GetPlayerByCharacterName(target);

            if (player == null)
            {
                return await Task.FromResult(new SaltyCommandResult(false, "Player is not connected on your server"));
            }

            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Context.Sender.CurrentMap, Context.Sender.Position.X, Context.Sender.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"Player will be teleported in {delay}ms"));
        }

        [Command("To")]
        [Description("Command that tries to delete an item by specifying its id and its position in the player's inventory.")]
        [Remarks("The two parameters are needed.")]
        public Task DeleteAsync([Description("ID of the item to delete.")]
            int id,
            [Description("Position of the item in the player's inventory.")]
            int position)
        {
            //do stuff
            Console.WriteLine($"Deleting the item with ID#{id} for the entity {Context.Sender.Id} at the inventory position {position}");

            return Task.CompletedTask;
        }
    }
}