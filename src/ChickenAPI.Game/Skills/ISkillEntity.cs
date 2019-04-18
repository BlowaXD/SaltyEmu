using System;
using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Skills
{
    public interface ISkillCapacity
    {
        /// <summary>
        ///     Skills
        /// </summary>
        IDictionary<long, SkillDto> Skills { get; }


        IDictionary<long, SkillDto> SkillsByCastId { get; }

        List<(DateTime, long)> CooldownsBySkillId { get; }

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

    public interface ISkillEntity : IEntity, ISkillCapacity
    {
    }
}