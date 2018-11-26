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
    [Description("Module related to entity teleportation. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class TeleportModule : SaltyModuleBase
    {
        [Command("ToMe")]
        [Description("Teleports the given character to you.")]
        public async Task<SaltyCommandResult> TeleportToMeAsync(
            [Description("Name of the character you want to teleport")] IPlayerEntity player,
            [Description("Amount of milliseconds to wait before teleporting it to you, by default no delay")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Context.Player.CurrentMap, Context.Player.Position.X, Context.Player.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"{player.Character.Name} has been teleported to you"));
        }

        [Command("To")]
        [Description("Teleports you to the given character.")]
        public async Task<SaltyCommandResult> TeleportToAsync(
            [Description("Name of the character you want to teleport to.")] IPlayerEntity player,
            [Description("Amount of milliseconds to wait before teleporting it to you, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Context.Player.CurrentMap, Context.Player.Position.X, Context.Player.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"You have been teleported to {player.Character.Name}."));
        }

        [Command("Map")]
        [Description("Teleports you to the specified map.")]
        public async Task<SaltyCommandResult> TeleportToMapAsync(
            [Description("Map on which you want to be teleported.")] IMapLayer map,
            [Description("Position X you want to be teleported.")] short x,
            [Description("Position Y you want to be teleported.")] short y,
            [Description("Amount of milliseconds to wait before teleporting you to the specified location, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            Context.Player.TeleportTo(map, x, y);
            return await Task.FromResult(new SaltyCommandResult(true, $"You have been teleported to the map #{map.Id} in positions x:{x}|y:{y}."));
        }
    }
}