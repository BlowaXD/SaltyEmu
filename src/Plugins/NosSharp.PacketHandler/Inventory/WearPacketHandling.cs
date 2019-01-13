using System.Threading.Tasks;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Client.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class WearPacketHandling : GenericGamePacketHandlerAsync<WearPacket>
    {
        protected override Task Handle(WearPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new InventoryWearEvent
            {
                ItemWearType = packet.WearPacketType == 0 ? ItemWearType.Player : ItemWearType.Partner,
                InventorySlot = packet.ItemSlot
            });
    }
}