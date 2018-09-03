using System.ComponentModel.DataAnnotations.Schema;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Models.BCard
{
    [Table("_data_skill_bcard")]
    public class SkillBCardModel : BCardModel
    {
        public SkillModel Skill { get; set; }

        [Column("SkillId")]
        [ForeignKey("FK_SKILLBCARD_TO_SKILL")]
        public long RelationId { get; set; }
    }
}