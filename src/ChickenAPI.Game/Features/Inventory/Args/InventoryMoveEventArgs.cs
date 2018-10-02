﻿using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Inventory.Args
{
    public class InventoryMoveEventArgs : ChickenEventArgs
    {
        public InventoryType InventoryType { get; set; }

        public short SourceSlot { get; set; }

        public short Amount { get; set; }

        public short DestinationSlot { get; set; }
    }
}