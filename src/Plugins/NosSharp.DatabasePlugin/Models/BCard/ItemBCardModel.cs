using System.ComponentModel.DataAnnotations.Schema;
using NosSharp.DatabasePlugin.Models.Item;

namespace NosSharp.DatabasePlugin.Models.BCard
{
    [Table("_data_item_bcard")]
    public class ItemBCardModel : BCardModel
    {
        public ItemModel Item { get; set; }

        [Column("ItemId")]
        [ForeignKey("FK_ITEMBCARD_TO_ITEM")]
        public long RelationId { get; set; }
    }
}