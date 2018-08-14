using System.Linq;
using System.Text;
using ChickenAPI.Game.Data.TransferObjects.Skills;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
{
    public static class SkiPacketExtensions
    {
        public static SkiPacket GenerateSkiPacket(this IPlayerEntity player)
        {
            var tmp = new StringBuilder();

            // check sp


            IOrderedEnumerable<SkillDto> skills = player.Skills.Skills.Values.OrderBy(s => s.CastId);
            
            foreach (SkillDto i in skills)
            {
                tmp.Append(' ');
                tmp.Append(i.Id);
            }

            return new SkiPacket
            {
                SkiPacketContent = tmp.ToString()
            };
        }
    }
}