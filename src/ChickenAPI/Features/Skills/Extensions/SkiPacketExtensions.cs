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

            // ski base
            // if no sp
            tmp.Append(200 + 20 * (byte)player.Character.Class);
            tmp.Append(' ');
            tmp.Append(201 + 20 * (byte)player.Character.Class);

            foreach (SkillDto i in skills)
            {
                tmp.Append(' ');
                tmp.Append(i.Id);
            }

            return new SkiPacket
            {
                SkiPacketContent = tmp.ToString().Trim()
            };
        }
    }
}