using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class PutPacketHandling
    {
        public static void OnPutPacket(PutPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new InventoryRemoveItemEvent
            {
                ItemInstance = player.Inventory.GetItemFromSlotAndType(packet.InventorySlot, packet.InventoryType),
                Amount = packet.Amount
            });
        }
    }
}