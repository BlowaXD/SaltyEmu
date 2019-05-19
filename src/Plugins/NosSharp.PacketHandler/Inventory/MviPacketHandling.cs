using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.ClientPackets.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class MviPacketHandling : GenericGamePacketHandlerAsync<MviPacket>
    {
        public MviPacketHandling(ILogger log) : base(log)
        {
        }

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