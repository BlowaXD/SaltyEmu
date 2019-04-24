using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.ServerPackets.Player;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class EqInfoPacketHandling : GenericGamePacketHandlerAsync<EquipmentInfoPacket>
    {
        public EqInfoPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(EquipmentInfoPacket packet, IPlayerEntity player)
        {
            await player.EmitEventAsync(new InventoryEqInfoEvent
            {
                Type = packet.Type,
                Slot = packet.Slot,
                ShopOwnerId = packet.ShopOwnerId
            });
        }
    }
}