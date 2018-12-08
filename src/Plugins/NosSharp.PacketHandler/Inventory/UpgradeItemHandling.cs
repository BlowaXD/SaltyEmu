using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Packets.Game.Client;

namespace NosSharp.PacketHandler
{
    public class UpgradeItemPaketHandling
    {
        private static readonly Logger Log = Logger.GetLogger<UpgradeItemPaketHandling>();

        public static void ItemUpgradePacket(UpgradePacket packet, IPlayerEntity session)
        {
            ItemInstanceDto item = session.Inventory.GetItemFromSlotAndType(packet.Slot, packet.InventoryType);

            if (item == null)
            {
                return;
            }

            if (packet.InventoryType2.HasValue)
            {
                ItemInstanceDto seconditem = session.Inventory.GetItemFromSlotAndType(packet.Slot2.Value, packet.InventoryType2.Value);
                if (seconditem == null)
                {
                    return;
                }
                if (packet.CellonInventoryType.HasValue)
                {
                    ItemInstanceDto cellonitem = session.Inventory.GetItemFromSlotAndType(packet.CellonSlot.Value, packet.CellonInventoryType.Value);

                    if (cellonitem == null)
                    {
                        return;
                    }

                    session.EmitEvent(new ItemUpgradeEventArgs
                    {
                        Type = packet.UpgradeType,
                        SecondItem = seconditem,
                        CellonItem = cellonitem
                    });
                    return;
                }
                session.EmitEvent(new ItemUpgradeEventArgs
                {
                    Type = packet.UpgradeType,
                    Item = item,
                    SecondItem = seconditem,
                });
                return;
            }

            session.EmitEvent(new ItemUpgradeEventArgs
            {
                Type = packet.UpgradeType,
                Item = item,
            });
        }
    }
}