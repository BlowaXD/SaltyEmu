using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using SaltyEmu.DatabasePlugin.Models.Item;

namespace SaltyEmu.DatabasePlugin.Models.Shop
{
    [Table("shop_recipe_item")]
    public class RecipeItemModel : IMappedModel
    {
        [Key]
        public long Id { get; set; }

        public byte Amount { get; set; }

        public ItemModel Item { get; set; }

        [ForeignKey(nameof(ItemId))]
        public long ItemId { get; set; }

        public RecipeModel Recipe { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public long RecipeId { get; set; }
    }
}