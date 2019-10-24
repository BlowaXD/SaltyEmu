using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.ClientPackets.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class RemovePacketHandling : GenericGamePacketHandlerAsync<RemovePacket>
    {
        public RemovePacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(RemovePacket packet, IPlayerEntity player)
        {
            await player.EmitEventAsync(new InventoryUnequipEvent
            {
                ItemToUnwear = player.Inventory.Wear[(int)packet.InventorySlot]
            });
        }
    }
}