using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Shops.Events
{
    public class ShopBuyEvent : GameEntityEvent
    {
        public VisualType Type { get; set; }

        public long OwnerId { get; set; }

        public short Slot { get; set; }

        public short Amount { get; set; }
    }
}