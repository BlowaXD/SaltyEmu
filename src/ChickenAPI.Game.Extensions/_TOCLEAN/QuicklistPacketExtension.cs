using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Skills.Extensions
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

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    CharacterQuicklistDto dto =
                        player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == j && n.Q2 == i && n.Morph == (player.HasSpWeared ? player.MorphId : 0));
                    if (dto == null)
                    {
                        if (j == 0)
                        {
                            tmp.Append("0.255.-1");
                            continue;
                        }

                        tmpOne.Append("7.7.-1");
                        continue;
                    }


                    string lel = $"{dto.Type}.{dto.Position}.{dto.Slot}";
                    if (j == 0)
                    {
                        tmp.Append(lel);
                    }
                    else
                    {
                        tmpOne.Append(lel);
                    }
                }
            }

            qslotZero.Data = tmp.ToString().Trim();
            qSlotOne.Data = tmpOne.ToString().Trim();
            return packets;
        }
    }
}