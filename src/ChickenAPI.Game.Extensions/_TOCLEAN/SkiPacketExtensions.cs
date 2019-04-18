using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Skills.Extensions
{
    public static class SkiPacketExtensions
    {
        public static IOrderedEnumerable<SkillDto> GetSpSkillsByCastIdAscending(this IPlayerEntity player)
        {
            return player.SkillComponent.Skills.Values.Where(s => s.Class == player.GetSpClassId()).OrderBy(s => s.CastId);
        }

        public static byte GetSpClassId(this IPlayerEntity player) => player.Sp.GetClassId();

        public static byte GetClassId(this ItemInstanceDto itemInstance) => (byte)(itemInstance.Item.Morph + 31);

        public static IOrderedEnumerable<SkillDto> GetSkillsByCastIdAscending(this IPlayerEntity player)
        {
            return player.SkillComponent.Skills.Values.OrderBy(s => s.CastId);
        }

        public static SkiPacket GenerateSkiPacket(this IPlayerEntity player)
        {
            IEnumerable<SkillDto> skills = player.IsTransformedSp ? GetSpSkillsByCastIdAscending(player) : GetSkillsByCastIdAscending(player);

            List<long> ids = new List<long>();

            if (!player.IsTransformedSp)
            {
                ids.Add(200 + 20 * (byte)player.Character.Class);
                ids.Add(201 + 20 * (byte)player.Character.Class);
            }
            else
            {
                ids.Add(skills.ElementAt(0).Id);
                ids.Add(skills.ElementAt(0).Id);
            }

            ids.AddRange(skills.Select(s => s.Id));

            return new SkiPacket
            {
                SkillIds = ids
            };
        }
    }
}