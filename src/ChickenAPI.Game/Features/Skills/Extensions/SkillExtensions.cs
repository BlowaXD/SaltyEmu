using System.Linq;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
{
    public static class SkillExtensions
    {
        public static int GetCp(this IPlayerEntity player)
        {
            if (player.Skills?.Skills?.Count == null)
            {
                return 0;
            }

            int cpMax = (player.Character.Class > CharacterClassType.Adventurer ? 40 : 0) + player.Experience.JobLevel * 2;
            int cpUsed = 0 + (int)player.Skills?.Skills?.Values.Where(s => s != null).Sum(dto => dto.CpCost);
            return cpMax - cpUsed;
        }
    }
}