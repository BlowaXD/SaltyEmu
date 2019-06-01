using ChickenAPI.Game._Events;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Shops.Events
{
    public class ShowShopEvent : GameEntityEvent
    {
        public VisualType Visual { get; set; }
        public long OwnerId { get; set; }
    }
}