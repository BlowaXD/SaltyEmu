using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Packets.Old.Game.Client.Npcs;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Npc.Dialog
{
    public class NRunPacketHandling : GenericGamePacketHandlerAsync<NrunPacket>
    {
        protected override Task Handle(NrunPacket packet, IPlayerEntity player) =>
            player.EmitEventAsync(new NpcDialogEvent
            {
                DialogId = packet.Runner,
                Type = packet.Type,
                Value = packet.Value,
                NpcId = packet.NpcId
            });
    }
}