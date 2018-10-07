using ChickenAPI.Data.Item;

namespace ChickenAPI.Data.Shop
{
    public class ShopItemDto : IMappedDto
    {
        public byte Color { get; set; }

        public long ItemId { get; set; }

        public sbyte Rare { get; set; }

        public long ShopId { get; set; }

        public byte Slot { get; set; }

        public byte Type { get; set; }

        public byte Upgrade { get; set; }

        public ItemDto Item { get; set; }
        public long Id { get; set; }
    }
}