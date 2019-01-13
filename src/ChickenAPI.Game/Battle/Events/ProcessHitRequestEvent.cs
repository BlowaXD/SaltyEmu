﻿using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Battle.Events
{
    public class ProcessHitRequestEvent : GameEntityEvent
    {
        public HitRequest HitRequest { get; set; }
    }
}