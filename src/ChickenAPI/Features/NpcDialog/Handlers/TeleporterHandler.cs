using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    public class TeleporterHandler
    {
        private static readonly Logger Log = Logger.GetLogger<TeleporterHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(301)]
        public static void OnGrahamDialogTeleport(IPlayerEntity player, NpcDialogEventArgs args)
        {
            Log.Info($"[TELEPORT][GRAHAM] {player.Character.Name}");
            // need to provide implementation
        }

        /// <summary>
        /// This method will teleport the requester to Krem or Alveus, depending on which dialog type he choosed
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(16)]
        public static void OnNpcDialogTeleport(IPlayerEntity player, NpcDialogEventArgs args)
        {
            switch (args.Type)
            {
                case 1:
                    TeleportZapMtKrem(player);
                    break;
                case 2:
                    TeleportZapPortsAlveus(player);
                    break;
            }
        }

        public static void TeleportZapMtKrem(IPlayerEntity player)
        {
            // TeleportPlayerOnMap(x,x,x);
            Log.Info($"J'ai était TP Au mt KREM ! .");
        }

        public static void TeleportZapPortsAlveus(IPlayerEntity player)
        {
            // TeleportPlayerOnMap(x,x,x);
            Log.Info($"J'ai était TP Au Ports Alveus .");
        }
    }
}