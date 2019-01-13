﻿using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Inventory.Events
{
    public class InventoryWearEvent : GameEntityEvent
    {
        public short InventorySlot { get; set; }
        public ItemWearType ItemWearType { get; set; }
    }
}