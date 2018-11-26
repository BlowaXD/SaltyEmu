using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums;
using ChickenAPI.Game.ECS.Entities;
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
                return await Task.FromResult(new SaltyCommandResult(false, $"{target} is not connected on your server"));
            }

            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Context.Sender.CurrentMap, Context.Sender.Position.X, Context.Sender.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"{target} has been teleported to you"));
        }

        [Command("To")]
        [Description("Command that teleports you to the given player")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> TeleportToAsync([Description("Name of the character you want to teleport to")]
            string target,
            [Description("Amount of milliseconds to wait before teleporting him to you, by default no delay")]
            int delay = 0)
        {
            var manager = ChickenContainer.Instance.Resolve<IPlayerManager>();
            IPlayerEntity player = manager.GetPlayerByCharacterName(target);

            if (player == null)
            {
                return await Task.FromResult(new SaltyCommandResult(false, $"{target} is not connected on your server"));
            }

            // wait for x ms
            await Task.Delay(delay);

            player.TeleportTo(Player.CurrentMap, Player.Position.X, Player.Position.Y);
            return await Task.FromResult(new SaltyCommandResult(true, $"You will be teleported to {target} in {delay}ms"));
        }

        [Command("Map")]
        [Description("Command that teleports you to g player to you")]
        [Remarks("Only the player paramter is needed")]
        public async Task<SaltyCommandResult> TeleportToMapAsync([Description("Name of the character you want to teleport to")]
            short mapId,
            [Description("Position X you want to be teleported")]
            short x,
            [Description("Position Y you want to be teleported")]
            short y,
            [Description("Amount of milliseconds to wait before teleporting him to you, by default no delay")]
            int delay = 0)
        {
            var manager = ChickenContainer.Instance.Resolve<IMapManager>();
            IMapLayer player = manager.GetBaseMapLayer(mapId);

            if (player == null)
            {
                return await Task.FromResult(new SaltyCommandResult(false, $"{mapId} does not exist"));
            }

            // wait for x ms
            await Task.Delay(delay);

            Player.TeleportTo(player, x, y);
            return await Task.FromResult(new SaltyCommandResult(true, $"You will be teleported to Map : {mapId} in {delay}ms"));
        }
    }
}