using System;
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
        [NpcDialog(1)]
        public static void OnNpcDialogTeleport(IPlayerEntity player, NpcDialogEventArgs dialog)
        {
            // example
        }

        [PermissionsRequirements("mabite")]
        [PermissionsRequirements(PermissionType.INVENTORY_ADD_ITEM)]
        public static void NpcDialogOnPluginReact(IPlayerEntity player, NpcDialogEventArgs dialog)
        {
            MethodInfo method = null;

            IEnumerable<PermissionsRequirementsAttribute> requirements = method.GetCustomAttributes<PermissionsRequirementsAttribute>();
            if (requirements.Any(requirement => !player.HasPermission(requirement)))
            {
                return;
            }
        }
    }
}