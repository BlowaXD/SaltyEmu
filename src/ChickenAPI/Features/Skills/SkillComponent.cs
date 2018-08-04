using System;
using System.Collections.Concurrent;
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

            Skills = new ConcurrentDictionary<long, SkillDto>();
            CooldownsBySkillId = new Queue<(DateTime, long)>();
        }

        public SkillComponent(IEntity entity, IEnumerable<SkillDto> skills) : this(entity)
        {
            foreach (SkillDto skill in skills)
            {
                Skills.TryAdd(skill.Id, skill);
            }
        }

        public ConcurrentDictionary<long, SkillDto> Skills { get; }

        public Queue<(DateTime, long)> CooldownsBySkillId { get; }

        public IEntity Entity { get; }
    }
}