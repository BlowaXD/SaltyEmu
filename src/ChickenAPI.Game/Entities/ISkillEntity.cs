using ChickenAPI.Game.Features.Skills;

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

        SkillComponent Skills { get; }
    }
}