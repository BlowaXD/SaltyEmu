using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class WearPacketHandling
    {
        public static void OnWearPacket(WearPacket packet, IPlayerEntity player)
        {
            player.NotifyEventHandler<InventoryEventHandler>(new InventoryWearEventArgs
            {
                InventoryType = packet.InventoryType,
                InventorySlot = packet.ItemSlot,
            });
        }

    }
}