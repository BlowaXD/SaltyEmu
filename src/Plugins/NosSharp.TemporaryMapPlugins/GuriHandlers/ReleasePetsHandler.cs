using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Events;
using ChickenAPI.Game.GuriHandling.Handling;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;

namespace SaltyEmu.BasicPlugin.GuriHandlers
{
    public class ReleasePetsHandler
    {
        private static readonly Logger Log = Logger.GetLogger<EmoticonGuriHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        [GuriEffect(300)]
        public static void ReleasePets(IPlayerEntity player, GuriEvent e)
        {
            if (e.Data != 8023)
            {
                return;
            }

            ItemInstanceDto item = player.Inventory.GetItemFromSlotAndType(e.InvSlot, InventoryType.Equipment);

            if (item == null)
            {
                return;
            }

            player.EmitEvent(new InventoryUseItemEvent
            {
                Item = item,
                Option = 50
            });

            Log.Info($"[GURI][RELEASE_PETS] {player.Character.Name} used Pearl : ");
        }
    }
}