using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Events;
using ChickenAPI.Game.GuriHandling.Handling;

namespace SaltyEmu.BasicPlugin.GuriHandlers
{
    public class ShellIdentifyingGuriHandler
    {
        // private static readonly Logger Log = Logger.GetLogger<ShellIdentifyingGuriHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        [GuriEffect(204)]
        public static void OnShellIdentifyRequest(IPlayerEntity player, GuriEvent e)
        {
            // Log.Info($"[GURI][SHELL_IDENTIFY] {player.Character.Name} identified shell : ");
        }
    }
}