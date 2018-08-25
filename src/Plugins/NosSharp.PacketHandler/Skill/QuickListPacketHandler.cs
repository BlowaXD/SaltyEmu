using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Quicklist;
using ChickenAPI.Game.Features.Quicklist.Args;
using ChickenAPI.Packets.Game.Client.QuickList;

namespace NosSharp.PacketHandler.Skill
{
    public class QuickListPacketHandling
    {
        public static void QuicklistPacketHandling(QsetPacket packet, IPlayerEntity player)
        {
            player.NotifyEventHandler<QuickListEventHandler>(new GenerateQuickListArgs
            {
                IsSkill = packet.IsSkill,
                Q1 = packet.Q1,
                Q2 = packet.Q2,
                Data1 = packet.Data1,
                Data2 = packet.Data2
            });
        }
    }
}