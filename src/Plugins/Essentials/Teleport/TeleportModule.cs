using ChickenAPI.Enums;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;
using System.Threading.Tasks;

namespace Essentials.Teleport
{
    [Name("Teleport")]
    [Description("Module related to entity teleportation. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class TeleportModule : SaltyModuleBase
    {
        [Command("Teleport")]
        [Description("Teleports you to the given character.")]
        public Task<SaltyCommandResult> TeleportToMeAsync(
            [Description("Name of the character you want to be teleported to.")] IPlayerEntity target,
            [Description("Amount of milliseconds to wait before teleporting it to you, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            Task.Delay(delay);

            Context.Player.TeleportTo(target.CurrentMap, target.Position.X, target.Position.Y);
            return Task.FromResult(new SaltyCommandResult(true, $"You have been teleported to {target.Character.Name}."));
        }

        [Command("Teleport")]
        [Description("Teleports the first given charactor to the second given character.")]
        public Task<SaltyCommandResult> TeleportToAsync(
            [Description("Name of the character you want to teleport.")] IPlayerEntity player,
            [Description("Name of the second character you want to teleport the first character to.")] IPlayerEntity target,
            [Description("Amount of milliseconds to wait before teleporting it to you, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            Task.Delay(delay);

            player.TeleportTo(target.CurrentMap, target.Position.X, target.Position.Y);
            return Task.FromResult(new SaltyCommandResult(true, $"You have been teleported to {player.Character.Name}."));
        }

        [Command("Teleport")]
        [Description("Teleports you to the specified map.")]
        public Task<SaltyCommandResult> TeleportToMapAsync(
            [Description("Map on which you want to be teleported.")] IMapLayer map,
            [Description("Position X you want to be teleported.")] short x,
            [Description("Position Y you want to be teleported.")] short y,
            [Description("Amount of milliseconds to wait before teleporting you to the specified location, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            Task.Delay(delay);

            Context.Player.TeleportTo(map, x, y);
            return Task.FromResult(new SaltyCommandResult(true, $"You have been teleported to the map #{map.Id} in positions x:{x}|y:{y}."));
        }

        [Command("Teleport")]
        [Description("Teleports the specified character to the specified map.")]
        public Task<SaltyCommandResult> TeleportToMapAsync(
            [Description("Name of the character you want to teleport.")] IPlayerEntity target,
            [Description("Map on which you want to be teleported.")] IMapLayer map,
            [Description("Position X you want to be teleported.")] short x,
            [Description("Position Y you want to be teleported.")] short y,
            [Description("Amount of milliseconds to wait before teleporting you to the specified location, by default no delay.")] int delay = 0)
        {
            // wait for x ms
            Task.Delay(delay);

            target.TeleportTo(map, x, y);
            return Task.FromResult(new SaltyCommandResult(true, $"You have been teleported to the map #{map.Id} in positions x:{x}|y:{y}."));
        }
    }
}