using System.Collections.Generic;
using System.Text;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
{
    public static class QuicklistPacketExtension
    {
        public static IEnumerable<QSlotPacket> GenerateQuicklistPacket(this IPlayerEntity player)
        {
            var qslotZero = new QSlotPacket { Slot = 0 };
            var qSlotOne = new QSlotPacket { Slot = 1 };
            List<QSlotPacket> packets = new List<QSlotPacket> { qslotZero, qSlotOne };

            var tmp = new StringBuilder();
            var tmpOne = new StringBuilder();
            for (int i = 0; i < 60; i++)
            {
                tmp.Append(' ');
                CharacterQuicklistDto dto;
                if (i < 30)
                {
                    if (!player.Quicklist.Q1.TryGetValue(i, out dto))
                    {
                        tmp.Append("0.255.-1");
                        continue;
                    }
                }
                else
                {
                    if (!player.Quicklist.Q2.TryGetValue(i, out dto))
                    {
                        tmp.Append("7.7.-1");
                        continue;
                    }
                }

                string lel = $"{(dto.IsSkill ? 0 : 1)}.{dto.EnumType}.{dto.RelatedSlot}";
                if (i < 30)
                {
                    tmp.Append(lel);
                }
                else
                {
                    tmpOne.Append(lel);
                }
            }

            qslotZero.Content = tmp.ToString().Trim();
            qSlotOne.Content = tmpOne.ToString().Trim();
            return packets;
        }
    }
}