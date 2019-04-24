using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.ClientPackets.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class MviPacketHandling : GenericGamePacketHandlerAsync<MviPacket>
    {
        protected override async Task Handle(MviPacket packet, IPlayerEntity player)
        {
            await player.EmitEventAsync(new InventoryMoveEvent
            {
                PocketType = packet.InventoryType,
                Amount = packet.Amount,
                SourceSlot = packet.Slot,
                DestinationSlot = packet.DestinationSlot
            });
        }
    }
}