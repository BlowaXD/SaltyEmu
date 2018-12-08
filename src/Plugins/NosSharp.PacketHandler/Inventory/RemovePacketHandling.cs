using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class RemovePacketHandling
    {
        public static void OnRemovePacket(RemovePacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new InventoryUnequipEvent
            {
                ItemToUnwear = player.Inventory.Wear[packet.InventorySlot]
            });
        }
    }
}