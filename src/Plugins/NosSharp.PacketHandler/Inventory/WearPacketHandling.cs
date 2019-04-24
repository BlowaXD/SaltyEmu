using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.ClientPackets.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class WearPacketHandling : GenericGamePacketHandlerAsync<WearPacket>
    {
        public WearPacketHandling(ILogger log) : base(log)
        {
        }

        protected override Task Handle(WearPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new InventoryWearEvent
            {
                ItemWearType = packet.Type == 0 ? ItemWearType.Player : ItemWearType.Partner,
                InventorySlot = packet.InventorySlot
            });
    }
}