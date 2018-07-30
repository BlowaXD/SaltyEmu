using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.AccessLayer.Repository;

namespace NosSharp.DatabasePlugin.Models.Shop
{
    [Table("shop_recipe")]
    public class RecipeModel : IMappedDto
    {
        [Key]
        public long Id { get; set; }

        public byte Amount { get; set; }

        public ItemModel Item { get; set; }

        [ForeignKey(nameof(ItemId))]
        public long ItemId { get; set; }

        public ShopModel Shop { get; set; }

        [ForeignKey(nameof(ShopId))]
        public long ShopId { get; set; }

        public IEnumerable<RecipeItemModel> RecipeItems { get; set; }
    }
}