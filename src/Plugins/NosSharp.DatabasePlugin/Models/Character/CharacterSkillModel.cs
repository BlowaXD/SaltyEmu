using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.AccessLayer.Repository;
using NosSharp.DatabasePlugin.Models.Skill;

namespace NosSharp.DatabasePlugin.Models.Character
{
    [Table("character_skill")]
    public class CharacterSkillModel : ISynchronizedDto
    {
        [Key]
        public Guid Id { get; set; }

        public CharacterModel Character { get; set; }

        [ForeignKey("FK_CHARACTERSKILL_TO_CHARACTER")]
        public long CharacterId { get; set; }

        public SkillModel Skill { get; set; }

        [ForeignKey("FK_CHARACTERSKILL_TO_SKILL")]
        public long SkillId { get; set; }
    }
}