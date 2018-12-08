using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Shops.Events
{
    public class ShopGetInformationEvent : ChickenEventArgs
    {
        public IEntity Shop { get; set; }

        public byte Type { get; set; }
    }
}