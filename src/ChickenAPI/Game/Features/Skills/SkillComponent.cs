using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Skills;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;

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
            foreach (var skill in skills)
            {
                Skills.TryAdd(skill.Id, skill);
            }
        }

        public IEntity Entity { get; }

        public ConcurrentDictionary<long, SkillDto> Skills { get; }

        public Queue<(DateTime, long)> CooldownsBySkillId { get; }
    }
}