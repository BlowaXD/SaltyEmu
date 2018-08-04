using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Game.Data.TransferObjects.Shop
{
    public class RecipeItemDto : IMappedDto
    {
        public long Id { get; set; }

        public short Amount { get; set; }

        public long ItemId { get; set; }

        public long RecipeId { get; set; }
    }
}
