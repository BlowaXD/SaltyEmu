using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Old.Game.Client.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class MviPacketHandling : GenericGamePacketHandlerAsync<MviPacket>
    {
        protected override Task Handle(MviPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new InventoryMoveEvent
            {
                PocketType = packet.InventoryType,
                Amount = packet.Amount,
                SourceSlot = packet.InventorySlot,
                DestinationSlot = packet.DestinationSlot
            });
    }
}