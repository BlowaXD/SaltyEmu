using System.Collections.Generic;
using ChickenAPI.Enums.Game.Items;

namespace ChickenAPI.Packets.Game.Server.Inventory
{
    [PacketHeader("inv")]
    public class InvPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1, RemoveSeparator = true, SeparatorBeforeProperty = " ", IsOptional = true)]
        public List<string> Items { get; set; }
    }
}