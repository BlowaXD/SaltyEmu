using System.Linq;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
{
    public static class SkillExtensions
    {
        public static int GetCp(this IPlayerEntity player)
        {
            int cpMax = (player.Character.Class > CharacterClassType.Adventurer ? 40 : 0) + player.Experience.JobLevel * 2;
            int cpUsed = player.Skills.Skills.Values.Aggregate(0, (current, s) => current + s.CpCost);
            return cpMax - cpUsed;
        }
    }
}