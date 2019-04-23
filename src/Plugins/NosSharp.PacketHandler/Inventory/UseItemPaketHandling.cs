using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Old.Game.Client.Inventory;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Inventory
{
    public class UseItemPaketHandling : GenericGamePacketHandlerAsync<UiPacket>
    {
        protected override async Task Handle(UiPacket packet, IPlayerEntity player)
        {
            ItemInstanceDto item = player.Inventory.GetItemFromSlotAndType(packet.InventorySlot, packet.InventoryType);

            if (item == null)
            {
                return;
            }

            string[] packetsplit = packet.OriginalContent.Split(' ', '^');

            await player.EmitEventAsync(new InventoryUseItemEvent
            {
                Item = item,
                Option = packetsplit[1].ElementAt(0) == '#' ? (byte)50 : (byte)0
            });
        }
    }
}