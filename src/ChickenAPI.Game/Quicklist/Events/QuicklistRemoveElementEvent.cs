﻿using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Quicklist.Events
{
    public class QuicklistRemoveElementEvent : GameEntityEvent
    {
        public short Q1 { get; set; }
        public short Q2 { get; set; }

        public short Data1 { get; set; }
        public short Data2 { get; set; }
    }
}