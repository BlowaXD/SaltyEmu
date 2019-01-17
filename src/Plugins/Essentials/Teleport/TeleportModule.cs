using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;
using System.Threading.Tasks;
using ChickenAPI.Game._ECS.Entities;

namespace Essentials.Teleport
{
    [Name("Teleport")]
    [Description("Module related to entity teleportation. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class TeleportModule : SaltyModuleBase
    {
        [Command("Teleport")]
        [Description("Teleports you to the given character.")]
        public async Task<SaltyCommandResult> TeleportToMeAsync(
            [Description("Name of the character you want to be teleported to.")] IPlayerEntity target,
            [Description("Amount of milliseconds to wait before teleporting it to you, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            await Context.Player.TeleportTo(target.CurrentMap, target.Position.X, target.Position.Y);
            return new SaltyCommandResult(true, $"You have been teleported to {target.Character.Name}.");
        }

        [Command("Teleport")]
        [Description("Teleports the first given charactor to the second given character.")]
        public async Task<SaltyCommandResult> TeleportToAsync(
            [Description("Name of the character you want to teleport.")] IPlayerEntity player,
            [Description("Name of the second character you want to teleport the first character to.")] IPlayerEntity target,
            [Description("Amount of milliseconds to wait before teleporting it to you, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            await player.TeleportTo(target.CurrentMap, target.Position.X, target.Position.Y);
            return new SaltyCommandResult(true, $"{target.Character.Name} have been teleported to {player.Character.Name}.");
        }

        [Command("Teleport")]
        [Description("Teleports you to the specified map.")]
        public async Task<SaltyCommandResult> TeleportToMapAsync(
            [Description("Map on which you want to be teleported.")] IMapLayer map,
            [Description("Position X you want to be teleported.")] short x,
            [Description("Position Y you want to be teleported.")] short y,
            [Description("Amount of milliseconds to wait before teleporting you to the specified location, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            await Context.Player.TeleportTo(map, x, y);
            return new SaltyCommandResult(true, $"You have been teleported to the map #{map.Map.Id} in positions x:{x}|y:{y}.");
        }

        [Command("Teleport")]
        [Description("Teleports the specified character to the specified map.")]
        public async Task<SaltyCommandResult> TeleportToMapAsync(
            [Description("Name of the character you want to teleport.")] IPlayerEntity target,
            [Description("Map on which you want to be teleported.")] IMapLayer map,
            [Description("Position X you want to be teleported.")] short x,
            [Description("Position Y you want to be teleported.")] short y,
            [Description("Amount of milliseconds to wait before teleporting you to the specified location, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            await Task.Delay(delay);

            await target.TeleportTo(map, x, y);
            return new SaltyCommandResult(true, $"{target.Character.Name} have been teleported to the map #{map.Map.Id} in positions x:{x}|y:{y}.");
        }
    }
}