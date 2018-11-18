using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Packets.Game.Client.Npcs;

namespace NosSharp.PacketHandler.Npc.Dialog
{
    public class NRunPacketHandling
    {
        public static void DialogPacket(NrunPacket packet, IPlayerEntity player)
        {
            player.EmitEvent(new NpcDialogEventArgs
            {
                DialogId = packet.Runner,
                Type     = packet.Type,
                Value    = packet.Value,
                NpcId    = packet.NpcId              
            });
        }
    }
}