using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Game.Features.Inventory.Packets;
using ChickenAPI.Game.Packets.Game.Client;

namespace NosSharp.PacketHandler.Inventory
{
    public class RemovePacketHandling
    {
        public static void OnRemovePacket(RemovePacket packet, IPlayerEntity player)
        {
            player.NotifyEventHandler<InventoryEventHandler>(new InventoryUnwearEventArgs
            {
                ItemToUnwear = player.Inventory.Wear[packet.InventorySlot]
            });
        }
    }
}