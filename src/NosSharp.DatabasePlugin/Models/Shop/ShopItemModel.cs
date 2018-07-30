using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.AccessLayer.Repository;

namespace NosSharp.DatabasePlugin.Models.Shop
{
    [Table("shop_item")]
    public class ShopItemModel : IMappedDto
    {
        [Key]
        public long Id { get; set; }

        public byte Color { get; set; }

        public ItemModel Item { get; set; }

        [ForeignKey(nameof(ItemId))]
        public long ItemId { get; set; }

        public sbyte Rare { get; set; }

        public ShopModel Shop { get; set; }

        [ForeignKey(nameof(ShopId))]
        public long ShopId { get; set; }

        public byte Slot { get; set; }

        public byte Type { get; set; }

        public byte Upgrade { get; set; }
    }
}
