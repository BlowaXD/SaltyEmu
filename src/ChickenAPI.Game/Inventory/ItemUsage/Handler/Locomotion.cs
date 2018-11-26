using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handlers
{
    public class LocomotionHandler
    {
        private static readonly Logger Log = Logger.GetLogger<LocomotionHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        //[PermissionsRequirements(PermissionType.INVENTORY_USE_ITEM)]
        [UseItemEffect(1000, ItemType.Special)]
        public static void Locomotion(IPlayerEntity player, InventoryUseItemEvent e)
        {
            Log.Info($"[LOCOMOTION] {player.Character.Name} used locomotion : ");
        }
    }
}