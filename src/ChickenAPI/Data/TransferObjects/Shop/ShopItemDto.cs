using ChickenAPI.Data.AccessLayer.Repository;

namespace ChickenAPI.Data.TransferObjects.Shop
{
    public class ShopItemDto : IMappedDto
    {
        public long Id { get; set; }

        public byte Color { get; set; }

        public long ItemId { get; set; }

        public sbyte Rare { get; set; }

        public long ShopId { get; set; }

        public byte Slot { get; set; }

        public byte Type { get; set; }

        public byte Upgrade { get; set; }
    }
}
