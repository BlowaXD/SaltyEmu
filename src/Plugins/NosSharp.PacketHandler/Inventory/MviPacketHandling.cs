using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Game.Packets.Game.Client;

namespace NosSharp.PacketHandler.Inventory
{
    public class MviPacketHandling
    {
        public static void OnMviPacket(MviPacket packet, IPlayerEntity player)
        {
            player.EntityManager.NotifySystem<InventorySystem>(player, new InventoryMoveEventArgs
            {
                InventoryType = packet.InventoryType,
                Amount = packet.Amount,
                SourceSlot = packet.InventorySlot,
                DestinationSlot = packet.DestinationSlot
            });
        }
    }
}