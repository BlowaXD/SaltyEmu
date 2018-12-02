using System;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.NosBazaar.Events
{
    public class NosBazaarDepositEvent : ChickenEventArgs
    {
        public ItemInstanceDto Item { get; set; }

        // public NosBazaarDepositTimeType { get; set; }

        // public long ExpectedPrice
    }
}
