using System.Linq;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
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
    }
}