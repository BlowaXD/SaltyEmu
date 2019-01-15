using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Shops.Events
{
    public class ShowShopEvent : GameEntityEvent
    {
        public VisualType Visual { get; set; }
        public long OwnerId { get; set; }
    }
}