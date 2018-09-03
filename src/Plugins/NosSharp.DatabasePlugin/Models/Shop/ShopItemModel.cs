using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using SaltyEmu.DatabasePlugin.Models.Item;

namespace SaltyEmu.DatabasePlugin.Models.Shop
{
    [Table("shop_item")]
    public class ShopItemModel : IMappedDto
    {
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

        [Key]
        public long Id { get; set; }
    }
}