﻿using System.Collections.Generic;

namespace ChickenAPI.Game.Packets.Game.Server.Inventory
{
    [PacketHeader("equip")]
    public class EquipmentPacket : PacketBase
    {

        [PacketIndex(0, RemoveSeparator = true)]
        public EqRareInfo EqRare { get; set; }

        [PacketIndex(1)]
        public string EqList { get; set; }
    }
}