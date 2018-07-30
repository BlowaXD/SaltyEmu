using ChickenAPI.Data.AccessLayer.Repository;

namespace ChickenAPI.Data.TransferObjects.Shop
{
    public class RecipeDto : IMappedDto
    {
        public long Id { get; set; }

        public byte Amount { get; set; }

        public long ItemId { get; set; }

        public long ShopId { get; set; }
    }
}
