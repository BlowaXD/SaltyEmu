using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog;
using ChickenAPI.Packets.Game.Client.Nrun;
using ChickenAPI.Game.Features.NpcDialog.Events;

namespace NosSharp.PacketHandler.Dialog
{
    public class NRunPacketHandling
    {
        public static void DialogPacket(NrunPacket packet, IPlayerEntity player)
        {
            player.NotifyEventHandler<NpcDialogEventHandler>(new NpcDialogEventArgs
            {
                DialogId = packet.Runner,
                Type     = packet.Type,
                Value    = packet.Value,
                NpcId    = packet.NpcId              
            });
        }
    }
}