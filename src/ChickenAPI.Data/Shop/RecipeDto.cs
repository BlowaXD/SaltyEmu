namespace ChickenAPI.Data.Shop
{
    public class RecipeDto : IMappedDto
    {
        public byte Amount { get; set; }

        public long ItemId { get; set; }

        public long ShopId { get; set; }
        public long Id { get; set; }
    }
}