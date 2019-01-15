using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Shops.Events
{
    public class ShopGetInformationEvent : GameEntityEvent
    {
        public IEntity Shop { get; set; }

        public byte Type { get; set; }
    }
}