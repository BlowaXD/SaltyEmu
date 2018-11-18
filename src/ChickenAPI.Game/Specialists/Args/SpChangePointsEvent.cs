﻿using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Specialists.Args
{
    public class SpChangePointsEvent : ChickenEventArgs
    {
        public short Attack { get; set; }
        public short Defense { get; set; }
        public short Element { get; set; }
        public short HpMp { get; set; }
    }
}