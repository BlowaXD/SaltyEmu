using System.Collections.Generic;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("inv")]
    public class InvPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1, SeparatorNestedElements = " ", SeparatorBeforeProperty = " ", IsOptional = true)]
        public List<string> Items { get; set; }
    }
}