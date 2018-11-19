using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Skills.Extensions
{
    public static class SkillExtensions
    {
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