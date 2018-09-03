using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.NpcDialog.Handlers
{
    public class TeleporterHandler
    {
        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(1, typeof(NpcDialogDelegate))]
        public static void OnNpcDialogTeleport(IPlayerEntity player, NpcDialogEventArgs dialog)
        {
        }
    }
}