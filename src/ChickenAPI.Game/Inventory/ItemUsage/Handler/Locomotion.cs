using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using ChickenAPI.Game.Locomotion.Events;

namespace ChickenAPI.Game.Inventory.ItemUsage.Handler
{
    public class LocomotionHandler
    {
        private static readonly Logger Log = Logger.GetLogger<LocomotionHandler>();

        /// <summary>
        ///
        ///
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        //[PermissionsRequirements(PermissionType.INVENTORY_USE_ITEM)]
        [UseItemEffect(1000, ItemType.Special)]
        public static void Locomotion(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (!player.IsTransformedLocomotion)
            {
                if (e.Option == 0)
                {
                    player.GenerateDelay(3000, DelayPacketType.Locomotion, $"#u_i^1^{player.Character.Id}^{(byte)e.Item.Type}^{e.Item.Slot}^2");
                    return;
                }
                player.EmitEvent(new LocomotionTransformEvent { Item = e.Item });
                return;
            }
            player.EmitEvent(new LocomotionUntransformEvent { });
        }
    }
}