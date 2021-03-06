﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Models.NpcMonster
{
    [Table("_data_npc_monster_skill")]
    public class NpcMonsterSkillModel : IMappedModel
    {
        /// <summary>
        ///     Can be considered as the skill vnum
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public SkillModel Skill { get; set; }

        [ForeignKey(nameof(SkillId))]
        public long SkillId { get; set; }

        public short Rate { get; set; }

        public NpcMonsterModel NpcMonster { get; set; }

        [ForeignKey(nameof(NpcMonsterId))]
        public long NpcMonsterId { get; set; }
    }
}