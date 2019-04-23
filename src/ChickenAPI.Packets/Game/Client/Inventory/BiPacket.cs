﻿using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Packets.Old.Attributes;

namespace ChickenAPI.Packets.Old.Game.Client.Inventory
{
    [PacketHeader("b_i")]
    public class BiPacket : PacketBase
    {
        [PacketIndex(0)]
        public InventoryType InventoryType { get; set; }

        [PacketIndex(1)]
        public short InventorySlot { get; set; }
    }
}