using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.ClientPackets.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class PutPacketHandling : GenericGamePacketHandlerAsync<PutPacket>
    {
        protected override async Task Handle(PutPacket packet, IPlayerEntity player)
        {
            await player.EmitEventAsync(new InventoryRemoveItemEvent
            {
                ItemInstance = player.Inventory.GetItemFromSlotAndType(packet.Slot, packet.PocketType),
                Amount = packet.Amount
            });
        }
    }
}