using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Data.AccessLayer.Repository;
using NosSharp.DatabasePlugin.Models.Skill;

namespace NosSharp.DatabasePlugin.Models.NpcMonster
{
    [Table("_data_npc_monster_skill")]
    public class NpcMonsterSkillModel : IMappedDto
    {
        /// <summary>
        /// Can be considered as the skill vnum
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public SkillModel Skill { get; set; }
        
        [ForeignKey("FK_NPCMONSTERSKILL_TO_SKILL")]
        public long SkillId { get; set; }

        public short Rate { get; set; }

        public NpcMonsterModel NpcMonster { get; set; }

        [ForeignKey("FK_NPCMONSTERSKILL_TO_NPCMONSTER")]
        public long NpcMonsterId { get; set; }
    }
}