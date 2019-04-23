using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Old.Game.Client.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class PutPacketHandling : GenericGamePacketHandlerAsync<PutPacket>
    {
        protected override Task Handle(PutPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new InventoryRemoveItemEvent
            {
                ItemInstance = player.Inventory.GetItemFromSlotAndType(packet.InventorySlot, packet.InventoryType),
                Amount = packet.Amount
            });
    }
}