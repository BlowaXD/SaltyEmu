using System;
using ChickenAPI.Data.Skills;

namespace ChickenAPI.Data.Character
{
    public class CharacterSkillDto : ISynchronizedDto
    {
        public long CharacterId { get; set; }

        public SkillDto Skill { get; set; }
        public long SkillId { get; set; }
        public Guid Id { get; set; }
    }
}