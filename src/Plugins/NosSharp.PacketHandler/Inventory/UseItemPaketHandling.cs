using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Game.Client.Inventory;
using System.Linq;
using ChickenAPI.Game.Inventory.Events;

namespace NosSharp.PacketHandler
{
    public class UseItemPaketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<UseItemPaketHandling>();

        public static void UseItemPacket(UiPacket packet, IPlayerEntity session)
        {
            ItemInstanceDto item = session.Inventory.GetItemFromSlotAndType(packet.InventorySlot, packet.InventoryType);

            if (item == null)
            {
                return;
            }

            string[] packetsplit = packet.OriginalContent.Split(' ', '^');

            session.EmitEvent(new InventoryUseItemEvent
            {
                Item = item,
                Option = packetsplit[1].ElementAt(0) == '#' ? (byte)50 : (byte)0
            });
        }
    }
}