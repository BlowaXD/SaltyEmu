using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Quicklist.Args;
using ChickenAPI.Packets.Game.Client.Player;

namespace NosSharp.PacketHandler.Skill
{
    public class QuickListPacketHandling
    {
        public static void QuicklistPacketHandling(QsetPacket packet, IPlayerEntity player)
        {
            if (packet.Data1.HasValue && packet.Data2.HasValue)
            {
                player.EmitEvent(new GenerateQuickListArgs
                {
                    Type = packet.Type,
                    Q1 = packet.Q1,
                    Q2 = packet.Q2,
                    Data1 = packet.Data1.Value,
                    Data2 = packet.Data2.Value
                });
            }
            else
            {
                player.EmitEvent(new GenerateQuickListArgs
                {
                    Type = packet.Type,
                    Q1 = packet.Q1,
                    Q2 = packet.Q2
                });
            }
        }

    }
}