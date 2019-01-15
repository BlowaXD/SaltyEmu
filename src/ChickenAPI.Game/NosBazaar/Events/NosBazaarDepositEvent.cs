using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.NosBazaar.Events
{
    public class NosBazaarDepositEvent : GameEntityEvent
    {
        public ItemInstanceDto Item { get; set; }

        // public NosBazaarDepositTimeType { get; set; }

        // public long ExpectedPrice
    }
}