using System;
using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;

namespace ChickenAPI.Game.Skills
{
    public interface ISkillEntity
    {
        /// <summary>
        ///     Skills
        /// </summary>
        IDictionary<long, SkillDto> Skills { get; }

        SkillComponent SkillComponent { get; }

        public Dictionary<Guid, CharacterSkillDto> CharacterSkills { get; }

        public Dictionary<long, SkillDto> Skills { get; }

        public Dictionary<long, SkillDto> SkillsByCastId { get; }

        public List<(DateTime, long)> CooldownsBySkillId { get; }

        /// <summary>
        ///     Checks if the entity has the given skill
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        bool HasSkill(long skillId);

        /// <summary>
        ///     Checks if the skill is under cooldown
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        bool CanCastSkill(long skillId);
    }
}