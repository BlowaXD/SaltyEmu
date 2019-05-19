using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Packets.ClientPackets.Npcs;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Npc.Dialog
{
    public class NRunPacketHandling : GenericGamePacketHandlerAsync<NrunPacket>
    {
        public NRunPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(NrunPacket packet, IPlayerEntity player)
        {
            await player.EmitEventAsync(new NpcDialogEvent
            {
                DialogId = (long)packet.Runner,
                Type = packet.Type.GetValueOrDefault(),
                Value = (int)packet.VisualType,
                NpcId = packet.VisualId.GetValueOrDefault()
            });
        }
    }
}