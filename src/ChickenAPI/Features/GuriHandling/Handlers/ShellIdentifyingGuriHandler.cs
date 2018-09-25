using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.GuriHandling.Args;
using ChickenAPI.Game.Features.GuriHandling.Handling;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.GuriHandling.Handlers
{
    public class ShellIdentifyingGuriHandler
    {
        private static readonly Logger Log = Logger.GetLogger<ShellIdentifyingGuriHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        [PermissionsRequirements(PermissionType.GURI_IDENTIFY_SHELL)]
        [GuriEffect(204)]
        public static void OnShellIdentifyRequest(IPlayerEntity player, GuriEventArgs e)
        {
            Log.Info($"[GURI][SHELL_IDENTIFY] {player.Character.Name} identified shell : ");
        }
    }
}