using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Shops
{
    public class PersonalShopItem
    {
        public long Price { get; set; }
        public short Quantity { get; set; }
        public ItemInstanceDto ItemInstance { get; set; }
    }
}