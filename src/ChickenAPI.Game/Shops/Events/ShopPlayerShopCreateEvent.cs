using System.Collections.Generic;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Shops.Events
{
    public class ShopPlayerShopCreateEvent : GameEntityEvent
    {
        public List<PersonalShopItem> ShopItems { get; set; }

        public string Name { get; set; }
    }
}