using ChickenAPI.Data.Skills;

namespace ChickenAPI.Data.NpcMonster
{
    public class NpcMonsterSkillDto : IMappedDto
    {

        /// <summary>
        ///     Can be considered as the skill vnum
        /// </summary>
        public long Id { get; set; }

        public SkillDto Skill { get; set; }

        public long SkillId { get; set; }
        public short Rate { get; set; }
        public long NpcMonsterId { get; set; }
    }
}