using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace ChickenAPI.Game.Skills.Extensions
{
    public static class SkillExtensions
    {
        private static readonly ISkillService SkillService = new Lazy<ISkillService>(ChickenContainer.Instance.Resolve<ISkillService>).Value;

        public static int GetCp(this IPlayerEntity player)
        {
            int cpMax = (player.Character.Class > CharacterClassType.Adventurer ? 40 : 0) + player.JobLevel * 2;
            if (player.SkillComponent?.Skills?.Count == null)
            {
                return cpMax;
            }

            int cpUsed = 0 + (int)player.SkillComponent?.Skills?.Values.Where(s => s != null).Sum(dto => dto.CpCost);
            return cpMax - cpUsed;
        }

        public static async Task LearnAdventurerSkillsAsync(this IPlayerEntity player)
        {
            if (player.Character.Class != CharacterClassType.Adventurer)
            {
                return;
            }

            IEnumerable<SkillDto> skills = await SkillService.GetByClassIdAsync((byte)player.Character.Class);
            foreach (SkillDto skillDto in skills.Where(s => s.LevelMinimum < player.JobLevel && s.Id >= 200 && s.Id != 209 && s.Id <= 210))
            {
                player.AddSkill(skillDto);
            }

            await player.ActualizeUiSkillList();
        }

        public static void AddSkill(this SkillComponent component, SkillDto skill)
        {
            if (skill == null)
            {
                return;
            }

            if (!component.Skills.ContainsKey(skill.Id))
            {
                component.Skills.Add(skill.Id, skill);
            }

            if (!component.SkillsByCastId.ContainsKey(skill.CastId))
            {
                component.SkillsByCastId.Add(skill.CastId, skill);
            }
        }

        public static async Task AddCharacterSkillAsync(this IPlayerEntity player, CharacterSkillDto skill)
        {
            if (player.SkillComponent.CharacterSkills.ContainsKey(skill.Id))
            {
                return;
            }

            player.SkillComponent.CharacterSkills.Add(skill.Id, skill);
            SkillDto skillDto = await SkillService.GetByIdAsync(skill.SkillId);
            player.SkillComponent.AddSkill(skillDto);
        }

        public static void AddSkills(this IPlayerEntity player, IEnumerable<SkillDto> skills)
        {
            foreach (SkillDto skill in skills)
            {
                player.AddSkill(skill);
            }
        }

        public static void AddSkill(this IPlayerEntity player, SkillDto skill)
        {
            player.SkillComponent.AddSkill(skill);
        }

        public static void RemoveSkillsByClassId(this IPlayerEntity player, byte classId)
        {
            IEnumerable<KeyValuePair<long, SkillDto>> skills = player.SkillComponent.Skills.Where(s => s.Value.Class == classId);

            foreach (KeyValuePair<long, SkillDto> pair in skills)
            {
                player.SkillComponent.Skills.Remove(pair.Value.Id);
                player.SkillComponent.SkillsByCastId.Remove(pair.Value.CastId);
            }
        }
    }
}