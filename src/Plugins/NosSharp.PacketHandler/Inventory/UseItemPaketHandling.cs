using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.ClientPackets.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class UseItemPaketHandling : GenericGamePacketHandlerAsync<UseItemPacket>
    {
        public UseItemPaketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(UseItemPacket packet, IPlayerEntity player)
        {
            ItemInstanceDto item = player.Inventory.GetItemFromSlotAndType(packet.Slot, packet.Type);

            if (item == null)
            {
                return;
            }

            // string[] packetsplit = packet.OriginalContent.Split(' ', '^');

            await player.EmitEventAsync(new InventoryUseItemEvent
            {
                Item = item,
                Option = packet.Header.StartsWith("#") ? (byte)50 : (byte)0 // packetsplit[1].ElementAt(0) == '#' ? (byte)50 : (byte)0
            });
        }
    }
}