using System;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Packets.Game.Client.Inventory;

namespace NosSharp.PacketHandler.Inventory
{
    public class WearPacketHandling
    {
        public static void OnWearPacket(WearPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new InventoryWearEvent
            {
                InventoryType = packet.InventoryType,
                InventorySlot = packet.ItemSlot
            });
        }
    }
}