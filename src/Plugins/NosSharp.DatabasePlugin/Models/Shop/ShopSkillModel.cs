using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Models.Shop
{
    [Table("shop_skill")]
    public class ShopSkillModel : IMappedDto
    {
        public SkillModel Skill { get; set; }

        [ForeignKey(nameof(SkillId))]
        public long SkillId { get; set; }

        public ShopModel Shop { get; set; }

        [ForeignKey(nameof(ShopId))]
        public long ShopId { get; set; }

        public byte Slot { get; set; }

        public byte Type { get; set; }

        [Key]
        public long Id { get; set; }
    }
}