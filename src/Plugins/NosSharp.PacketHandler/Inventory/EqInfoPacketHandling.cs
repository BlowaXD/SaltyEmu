using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Old.Game.Server.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class EqInfoPacketHandling : GenericGamePacketHandlerAsync<EquipmentInfoPacket>
    {
        protected override Task Handle(EquipmentInfoPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new InventoryEqInfoEvent
            {
                Type = packet.Type,
                Slot = packet.Slot,
                ShopOwnerId = packet.ShopOwnerId
            });
    }
}