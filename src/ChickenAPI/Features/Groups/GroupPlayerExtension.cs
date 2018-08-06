using System.Collections.Generic;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Groups
{
    public static class GroupPlayerExtension
    {
        public static IEnumerable<PstPacket> GeneratePstPacket(this IPlayerEntity player)
        {
            var group = player.GetComponent<GroupComponent>();
            if (group == null)
            {
                return null;
            }

            List<PstPacket> tmp = new List<PstPacket>();
            int i = 0;

            foreach (IPlayerEntity member in group.Members)
            {
                if (player == member)
                {
                    // partner & pet
                }

                tmp.Add(new PstPacket
                {
                    VisualType = VisualType.Character,
                    GroupIndex = i,
                    Gender = member.Character.Gender,
                    VisualId = member.Character.Id,
                    Class = member.Character.Class,
                    HpPercentage = member.Battle.HpPercentage,
                    HpMax = member.Battle.HpMax,
                    MpPercentage = member.Battle.MpPercentage,
                    MpMax = member.Battle.MpMax,
                    Morph = member.Character.Morph,
                    Buffs = ""
                });
            }

            return tmp;
        }
    }
}