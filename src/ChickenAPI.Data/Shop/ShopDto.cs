namespace ChickenAPI.Data.Shop
{
    public class ShopDto : IMappedDto
    {
        public long MapNpcId { get; set; }

        public string Name { get; set; }

        public byte MenuType { get; set; }

        public byte ShopType { get; set; }
        public long Id { get; set; }
    }
}