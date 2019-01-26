using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.ItemUsage.Handling;
using ChickenAPI.Game.Locomotion.Events;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace SaltyEmu.BasicPlugin.ItemUsageHandlers.Handler
{
    public class WingsSpCardHandler
    {
        private static readonly Logger Log = Logger.GetLogger<WingsSpCardHandler>();

        [UseItemEffect(650, ItemType.Special)]
        public static void Wings(IPlayerEntity player, InventoryUseItemEvent e)
        {
            if (player.IsTransformedSp)
            {
                if (e.Option == 0)
                {
                    player.GenerateQna((new UiPacket
                    {
                        VisualType = VisualType.Character,
                        CharacterId = player.Id,
                        InventoryType = e.Item.Type,
                        InventorySlot = (byte)e.Item.Slot,
                        Unknown2 = 3,
                        Unknown3 = 0
                    }), "ASK_WINGS_CHANGE");
                    return;
                }

                // EmitEvent(new ChangeSpWings)
                return;
            }
        }
    }
}