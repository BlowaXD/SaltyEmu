using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillComponent : IComponent
    {
        private static ISkillService _skillService;

        public SkillComponent(IEntity entity)
        {
            Entity = entity;

            Skills = new Dictionary<long, SkillDto>();
            CooldownsBySkillId = new List<(DateTime, long)>();

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            int tmp = 200 + 20 * (byte)player.Character.Class;
            Skills.Add(tmp, SkillService.GetById(tmp));
            Skills.Add(tmp + 1, SkillService.GetById(tmp + 1));

            if (player.Character.Class == CharacterClassType.Adventurer)
            {
                Skills.Add(tmp + 9, SkillService.GetById(tmp + 9));
            }
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

        private static ISkillService SkillService => _skillService ?? (_skillService = ChickenContainer.Instance.Resolve<ISkillService>());

        public Dictionary<long, SkillDto> Skills { get; }

        public List<(DateTime, long)> CooldownsBySkillId { get; }

        public IEntity Entity { get; }
    }
}