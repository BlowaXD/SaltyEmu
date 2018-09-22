using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    public class TeleporterHandler
    {
        private static readonly Logger Log = Logger.GetLogger<TeleporterHandler>();

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(301, typeof(TeleporterHandler))]
        public static void OnGrahamDialogTeleport(IPlayerEntity player, NpcDialogEventArgs args)
        {
            TeleportGraham(player);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(16, typeof(TeleporterHandler))]
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

        public static void TeleportGraham(IPlayerEntity player)
        {
            // TeleportPlayerOnMap(x,x,x);
            Log.Info($"J'ai était Tp par Graham LUL .");
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