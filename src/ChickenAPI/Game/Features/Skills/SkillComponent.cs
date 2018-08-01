using System;
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

            Skills = new Dictionary<long, SkillDto>();
            CooldownsBySkillId = new Queue<(DateTime, long)>();
        }

        public IEntity Entity { get; }

        public Dictionary<long, SkillDto> Skills { get; }

        public Queue<(DateTime, long)> CooldownsBySkillId { get; }
    }
}