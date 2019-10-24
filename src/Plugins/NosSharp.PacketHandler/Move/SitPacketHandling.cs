using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements.Events;
using ChickenAPI.Packets.ClientPackets.Movement;
using ChickenAPI.Packets.Enumerations;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Move
{
    public class SitPacketHandling : GenericGamePacketHandlerAsync<SitPacket>
    {
        public SitPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(SitPacket packet, IPlayerEntity player)
        {
            foreach (SitSubPacket u in packet.Users)
            {
                if (u.VisualType == VisualType.Player && u.VisualId != player.Character.Id)
                {
                    // return;
                    return;
                }

                //TODO: rest on mate
            }

            await player.EmitEventAsync(new TriggerSitEvent
            {
                ChildsId = packet.Users.Select(s => s.VisualId)
            });
        }
    }
}