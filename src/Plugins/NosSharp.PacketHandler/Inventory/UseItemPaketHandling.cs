using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler
{
    public class UseItemPaketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<UseItemPaketHandling>();

        public static void GuriPacket(UiPacket packet, IPlayerEntity session)
        {
            ItemInstanceDto item = session.Inventory.GetItemFromSlotAndType(packet.InventorySlot, packet.InventoryType);

            if (item == null)
            {
                return;
            }

            session.EmitEvent(new InventoryUseItemEvent
            {
                Item = item
            });
        }
    }
}