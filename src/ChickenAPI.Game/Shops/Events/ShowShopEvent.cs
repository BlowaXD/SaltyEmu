using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Shops.Args
{
    public class ShowShopEvent : GameEntityEvent
    {
        public VisualType Visual { get; set; }
        public long OwnerId { get; set; }
    }
}