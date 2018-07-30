using ChickenAPI.Data.AccessLayer.Repository;

namespace ChickenAPI.Data.TransferObjects.Shop
{
    public class ShopDto : IMappedDto
    {
        public long Id { get; set; }

        public long MapNpcId { get; set; }

        public string Name { get; set; }

        public byte MenuType { get; set; }

        public byte ShopType { get; set; }
    }
}
