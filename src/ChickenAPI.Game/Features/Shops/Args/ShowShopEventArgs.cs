using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Game.Features.Shops.Args
{
    public class ShowShopEventArgs : ChickenEventArgs
    {
        public VisualType Visual { get; set; }
        public long OwnerId { get; set; }
    }
}