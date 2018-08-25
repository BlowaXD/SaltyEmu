using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using NosSharp.DatabasePlugin.Models.Item;

namespace NosSharp.DatabasePlugin.Models.Shop
{
    [Table("shop_recipe_item")]
    public class RecipeItemModel : IMappedDto
    {
        public byte Amount { get; set; }

        public ItemModel Item { get; set; }

        [ForeignKey(nameof(ItemId))]
        public long ItemId { get; set; }

        public RecipeModel Recipe { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public long RecipeId { get; set; }

        [Key]
        public long Id { get; set; }
    }
}