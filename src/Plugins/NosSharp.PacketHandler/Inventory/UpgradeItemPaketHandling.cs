using System.Threading.Tasks;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Packets.Game.Client;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class UpgradeItemPaketHandling : GenericGamePacketHandlerAsync<UpgradePacket>
    {
        protected override async Task Handle(UpgradePacket packet, IPlayerEntity player)
        {
            ItemInstanceDto item = player.Inventory.GetItemFromSlotAndType(packet.Slot, packet.InventoryType);

            if (item == null)
            {
                return;
            }

            if (!packet.InventoryType2.HasValue || !packet.Slot2.HasValue)
            {
                await player.EmitEventAsync(new ItemUpgradeEvent
                {
                    Type = packet.UpgradeType,
                    Item = item
                });
                return;
            }

            ItemInstanceDto seconditem = player.Inventory.GetItemFromSlotAndType(packet.Slot2.Value, packet.InventoryType2.Value);
            if (seconditem == null)
            {
                return;
            }

            if (!packet.CellonInventoryType.HasValue || !packet.CellonSlot.HasValue)
            {
                await player.EmitEventAsync(new ItemUpgradeEvent
                {
                    Type = packet.UpgradeType,
                    Item = item,
                    SecondItem = seconditem
                });
                return;
            }

            ItemInstanceDto cellonitem = player.Inventory.GetItemFromSlotAndType(packet.CellonSlot.Value, packet.CellonInventoryType.Value);

            if (cellonitem == null)
            {
                return;
            }

            await player.EmitEventAsync(new ItemUpgradeEvent
            {
                Type = packet.UpgradeType,
                SecondItem = seconditem,
                CellonItem = cellonitem
            });
        }
    }
}