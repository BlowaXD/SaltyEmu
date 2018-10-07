using System;
using ChickenAPI.Data.Skills;

namespace ChickenAPI.Data.Character
{
    public class CharacterSkillDto : ISynchronizedDto
    {
        public Guid Id { get; set; }
        public long CharacterId { get; set; }

        public SkillDto Skill { get; set; }
        public long SkillId { get; set; }
    }
}