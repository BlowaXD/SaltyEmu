using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Game.Data.TransferObjects.Shop
{
    public class RecipeItemDto : IMappedDto
    {
        public short Amount { get; set; }

        public long ItemId { get; set; }

        public long RecipeId { get; set; }
        public long Id { get; set; }
    }
}