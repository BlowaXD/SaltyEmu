using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.AccessLayer.Repository;
using NosSharp.DatabasePlugin.Models.BCard;
using NosSharp.DatabasePlugin.Models.NpcMonster;

namespace NosSharp.DatabasePlugin.Models.Skill
{
    [Table("_data_skill")]
    public class SkillModel : IMappedDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public short AttackAnimation { get; set; }

        public short CastAnimation { get; set; }

        public short CastEffect { get; set; }

        public short CastId { get; set; }

        public short CastTime { get; set; }

        public byte Class { get; set; }

        public short Cooldown { get; set; }

        public byte CpCost { get; set; }

        public short Duration { get; set; }

        public short Effect { get; set; }

        public byte Element { get; set; }

        public byte HitType { get; set; }

        public short ItemVNum { get; set; }

        public byte Level { get; set; }

        public byte LevelMinimum { get; set; }

        public byte MinimumAdventurerLevel { get; set; }

        public byte MinimumArcherLevel { get; set; }

        public byte MinimumMagicianLevel { get; set; }

        public byte MinimumSwordmanLevel { get; set; }

        public short MpCost { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public byte Range { get; set; }

        public byte SkillType { get; set; }

        public byte TargetRange { get; set; }

        public byte TargetType { get; set; }

        public byte Type { get; set; }

        public short UpgradeSkill { get; set; }

        public short UpgradeType { get; set; }

        public ICollection<SkillBCardModel> BCards { get; set; }
        public IEnumerable<NpcMonsterSkillModel> NpcMonsterSkills { get; set; }
    }
}