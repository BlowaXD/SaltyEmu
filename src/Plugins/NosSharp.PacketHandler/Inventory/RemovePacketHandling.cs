using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class RemovePacketHandling
    {
        public static void OnRemovePacket(RemovePacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new InventoryUnwearEventArgs
            {
                ItemToUnwear = player.Inventory.Wear[packet.InventorySlot]
            });
        }
    }
}