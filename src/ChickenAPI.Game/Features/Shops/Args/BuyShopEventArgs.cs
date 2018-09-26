using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class BuyShopEventArgs : ChickenEventArgs
    {
        public VisualType Type { get; set; }

        public long OwnerId { get; set; }

        public short Slot { get; set; }

        public short Amount { get; set; }
    }
}