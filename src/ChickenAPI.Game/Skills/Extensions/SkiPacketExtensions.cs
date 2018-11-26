using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            return player.SkillComponent.Skills.Values.Where(s => s.Class == player.GetClassId()).OrderBy(s => s.CastId);
        }

        public static byte GetClassId(this IPlayerEntity player)
        {
            return player.Sp.GetClassId();
        }

        public static byte GetClassId(this ItemInstanceDto itemInstance)
        {
            return (byte)(itemInstance.Item.Morph + 31);
        }

        public static IOrderedEnumerable<SkillDto> GetSkillsByCastIdAscending(this IPlayerEntity player)
        {
            return player.SkillComponent.Skills.Values.OrderBy(s => s.CastId);
        }

        public static SkiPacket GenerateSkiPacket(this IPlayerEntity player)
        {
            var tmp = new StringBuilder();

            // check sp


            IEnumerable<SkillDto> skills = player.HasSpWeared ? GetSpSkillsByCastIdAscending(player) : GetSkillsByCastIdAscending(player);

            // ski base
            // if no sp
            if (player.HasSpWeared)
            {
                tmp.Append(skills.ElementAt(0).Id + ' ' + skills.ElementAt(0).Id);
            }
            else
            {
                tmp.Append(200 + 20 * (byte)player.Character.Class + ' ' + 201 + 20 * (byte)player.Character.Class);
            }

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