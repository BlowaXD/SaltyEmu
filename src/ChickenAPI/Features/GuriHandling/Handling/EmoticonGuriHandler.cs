using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.GuriHandling.Args;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.GuriHandling.Handling
{
    public class EmoticonGuriHandler
    {

        private static readonly Logger Log = Logger.GetLogger<TeleporterHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        [PermissionsRequirements(PermissionType.GURI_EMOTICON)]
        [GuriEffect(301)]
        public static void OnEmoticonRequest(IPlayerEntity player, GuriEventArgs args)
        {
            Log.Info($"[GURI][ALT] {player.Character.Name} used emoji : " );
        }

    }
}