using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class EqInfoPacketHandling
    {
        public static void OnEqInfoPacket(EquipmentInfoPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new InventoryEqInfoEventArgs
            {
                Type = packet.Type,
                Slot = packet.Slot,
                ShopOwnerId = packet.ShopOwnerId
            });
        }
    }
}
