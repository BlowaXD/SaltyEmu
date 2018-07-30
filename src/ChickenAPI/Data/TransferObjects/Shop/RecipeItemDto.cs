using ChickenAPI.Data.AccessLayer.Repository;

namespace ChickenAPI.Data.TransferObjects.Shop
{
    public class RecipeItemDto : IMappedDto
    {
        public long Id { get; set; }

        public short Amount { get; set; }

        public long ItemId { get; set; }

        public long RecipeId { get; set; }
    }
}
