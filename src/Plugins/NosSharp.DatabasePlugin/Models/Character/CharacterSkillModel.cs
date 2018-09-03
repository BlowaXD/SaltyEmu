using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Models.Character
{
    [Table("character_skill")]
    public class CharacterSkillModel : ISynchronizedDto
    {
        public CharacterModel Character { get; set; }

        [ForeignKey("FK_CHARACTERSKILL_TO_CHARACTER")]
        public long CharacterId { get; set; }

        public SkillModel Skill { get; set; }

        [ForeignKey("FK_CHARACTERSKILL_TO_SKILL")]
        public long SkillId { get; set; }

        [Key]
        public Guid Id { get; set; }
    }
}