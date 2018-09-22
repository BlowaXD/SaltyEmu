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
        [NpcDialogHandler(1, typeof(TeleporterHandler))]
        public static void OnNpcDialogTeleport(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // do the work
            switch (args.DialogId)
            {
                case 301:
                    TeleportGraham(player);
                    break;

                case 16:
                    switch (args.Type)
                    {
                        case 1:
                            TeleportZapMtKrem(player);
                            break;
                        case 2:
                            TeleportZapPortsAlveus(player);
                            break;
                        default:
                            return;
                    }
                    break;

                default:
                    Log.Info($" No Handler for {args.DialogId} {args.Type} {args.Value} {args.NpcId} : Mashalux Beaugosse va .");
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

        // public static void TeleportPlayerOnMap( mapid , x , y)
    }
}