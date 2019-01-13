using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Shops
{
    public class PersonalShop
    {
        public PersonalShop(IPlayerEntity player, long id)
        {
            Id = id;
            Owner = player;
        }

        public long Id { get; }

        public IPlayerEntity Owner { get; }
        public string ShopName { get; set; }
        public IEnumerable<PersonalShopItem> ShopItems { get; set; }
    }
}