using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    public class TeleporterHandler
    {
        [NpcDialog(1)]
        public static void OnNpcDiaLogTeleport(IPlayerEntity player, NpcDialogEventArgs dialog)
        {
            // example
        }
    }
}