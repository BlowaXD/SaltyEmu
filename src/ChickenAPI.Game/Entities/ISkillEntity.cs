using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Skills;

namespace ChickenAPI.Game.Entities
{
    public interface ISkillEntity
    {
        /// <summary>
        /// Checks if the entity has the given skill
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        bool HasSkill(long skillId);

        /// <summary>
        /// Checks if the skill is under cooldown
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        bool CanCastSkill(long skillId);

        /// <summary>
        /// Skills
        /// </summary>
        IDictionary<long, SkillDto> Skills { get; }

        SkillComponent SkillComponent { get; }
    }
}