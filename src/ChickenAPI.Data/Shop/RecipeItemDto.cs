namespace ChickenAPI.Data.Shop
{
    public class RecipeItemDto : IMappedDto
    {
        public short Amount { get; set; }

        public long ItemId { get; set; }

        public long RecipeId { get; set; }
        public long Id { get; set; }
    }
}