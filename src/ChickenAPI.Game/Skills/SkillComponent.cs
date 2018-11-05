using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Skills
{
    public class SkillComponent : IComponent
    {
        public SkillComponent(IEntity entity)
        {
            Entity = entity;

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            int tmp = 200 + 20 * (byte)player.Character.Class;
            AddSkill(SkillService.GetById(tmp));
            AddSkill(SkillService.GetById(tmp + 1));

            if (player.Character.Class == CharacterClassType.Adventurer)
            {
                AddSkill(SkillService.GetById(tmp + 9));
            }
        }

        private void AddSkill(SkillDto skill)
        {
            if (!Skills.ContainsKey(skill.Id))
            {
                Skills.Add(skill.Id, skill);
            }

            if (!SkillsByCastId.ContainsKey(skill.CastId))
            {
                SkillsByCastId.Add(skill.CastId, skill);
            }
        }

        public SkillComponent(IEntity entity, IEnumerable<CharacterSkillDto> skills) : this(entity)
        {
            if (skills == null)
            {
                return;
            }

            foreach (CharacterSkillDto characterSkill in skills)
            {
                CharacterSkills.Add(characterSkill.Id, characterSkill);
                AddSkill(characterSkill.Skill);
            }
        }

        public Dictionary<Guid, CharacterSkillDto> CharacterSkills { get; } = new Dictionary<Guid, CharacterSkillDto>();

        private static readonly ISkillService SkillService = new Lazy<ISkillService>(() => ChickenContainer.Instance.Resolve<ISkillService>()).Value;

        public Dictionary<long, SkillDto> Skills { get; } = new Dictionary<long, SkillDto>();

        public Dictionary<long, SkillDto> SkillsByCastId { get; } = new Dictionary<long, SkillDto>();

        public List<(DateTime, long)> CooldownsBySkillId { get; } = new List<(DateTime, long)>();

        public IEntity Entity { get; }
    }
}