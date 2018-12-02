using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Shops
{
    public class PersonalShopItem
    {
        public int Slot { get; set; }
        public long Price { get; set; }
        public short Quantity { get; set; }
        public ItemInstanceDto ItemInstance { get; set; }
    }
}