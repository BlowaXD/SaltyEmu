using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Events;
using ChickenAPI.Packets.ClientPackets.Player;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Guri
{
    public class GuriPacketHandling : GenericGamePacketHandlerAsync<ClientGuriPacket>
    {
        protected override async Task Handle(ClientGuriPacket packet, IPlayerEntity player)
        {
            string[] packetsplit = new[] { "", "" }; // todo;
            if (packetsplit[1].ElementAt(0) == '#')
            {
                if (!packet.VisualId.HasValue)
                {
                    return;
                }

                await player.EmitEventAsync(new GuriEvent
                {
                    EffectId = packet.Type,
                    Data = (short)packet.Argument,
                    InvSlot = (short)packet.VisualId.Value
                });
                return;
            }

            await player.EmitEventAsync(new GuriEvent
            {
                EffectId = packet.Type,
                Data = (short)packet.Data
            });
        }
    }
}