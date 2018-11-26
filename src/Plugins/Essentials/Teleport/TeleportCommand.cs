using System.Threading.Tasks;
using ChickenAPI.Enums;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
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
            IPlayerEntity player,
            [Description("Amount of milliseconds to wait before teleporting him to you, by default no delay")]
            int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Context.Player.CurrentMap, Context.Player.Position.X, Context.Player.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name} has been teleported to you"));
        }

        [Command("To")]
        [Description("Command that teleports you to the given player")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> TeleportToAsync([Description("Name of the character you want to teleport to")]
            IPlayerEntity player,
            [Description("Amount of milliseconds to wait before teleporting him to you, by default no delay")]
            int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Context.Player.CurrentMap, Context.Player.Position.X, Context.Player.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"You will be teleported to {player.Character.Name} in {delay}ms"));
        }

        [Command("Map")]
        [Description("Command that teleports you to g player to you")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> TeleportToMapAsync([Description("Map on which you want to be teleported")]
            IMapLayer map,
            [Description("Position X you want to be teleported")]
            short x,
            [Description("Position Y you want to be teleported")]
            short y,
            [Description("Amount of milliseconds to wait before teleporting you to the specified location, by default no delay")]
            int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            Context.Player.TeleportTo(map, x, y);
            return await Task.FromResult(new SaltyCommandResult(true, $"You will be teleported to Map: {map.Id} in {delay}ms"));
        }
    }
}