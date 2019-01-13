using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Client.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class RemovePacketHandling : GenericGamePacketHandlerAsync<RemovePacket>
    {
        protected override Task Handle(RemovePacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new InventoryUnequipEvent
            {
                ItemToUnwear = player.Inventory.Wear[packet.InventorySlot]
            });
    }
}