using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillComponent : IComponent
    {
        private static ISkillService _skillService;

        private static ISkillService Service => _skillService ?? (_skillService = Container.Instance.Resolve<ISkillService>());
        public SkillComponent(IEntity entity)
        {
            Entity = entity;

            Skills = new Dictionary<long, SkillDto>();
            CooldownsBySkillId = new Queue<(DateTime, long)>();

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            int tmp = 200 + 20 * (byte)player.Character.Class;
            Skills.Add(tmp, Service.GetById(tmp));
            Skills.Add(tmp + 1, Service.GetById(tmp + 1));

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