using System.Collections;
using System.Collections.Generic;
using System.Text;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Quicklist;
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
                CharacterQuicklistDto dto = player.Quicklist.Quicklist[i];
                if (i < 30)
                {
                    if (dto == null)
                    {
                        tmp.Append("0.255.-1");
                        continue;
                    }
                }
                else
                {
                    if (dto == null)
                    {
                        tmp.Append("7.7.-1");
                        continue;
                    }

                }

                string lel = $"{dto.Type}.{dto.Position}.{dto.Slot}";
                if (i < 30)
                {
                    tmp.Append(lel);
                }
                else
                {
                    tmpOne.Append(lel);
                }
            }

            qslotZero.Data = tmp.ToString().Trim();
            qSlotOne.Data = tmpOne.ToString().Trim();
            return packets;
        }
    }
}