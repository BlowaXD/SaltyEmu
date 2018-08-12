using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Skills;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillComponent : IComponent
    {
        public SkillComponent(IEntity entity)
        {
            Entity = entity;

            Skills = new Dictionary<long, SkillDto>();
            CooldownsBySkillId = new Queue<(DateTime, long)>();
        }

        public SkillComponent(IEntity entity, IEnumerable<SkillDto> skills) : this(entity)
        {
            foreach (SkillDto skill in skills)
            {
                if (!Skills.ContainsKey(skill.Id))
                {
                    Skills.Add(skill.Id, skill);
                }
            }
        }

        public Dictionary<long, SkillDto> Skills { get; }

        public Queue<(DateTime, long)> CooldownsBySkillId { get; }

        public IEntity Entity { get; }
    }
}