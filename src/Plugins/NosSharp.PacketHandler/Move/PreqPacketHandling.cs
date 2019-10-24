using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Portals.Events;
using ChickenAPI.Packets.ClientPackets.Movement;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.Move
{
    public class PreqPacketHandling : GenericGamePacketHandlerAsync<PreqPacket>
    {
        public PreqPacketHandling(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(PreqPacket packet, IPlayerEntity player)
        {
            PortalDto currentPortal = player.CurrentMap.Map.GetPortalFromPosition(player.Position.X, player.Position.Y);

            if (currentPortal == null)
            {
                Log.Warn($"Cannot find a valid portal at {player.Position.X}x{player.Position.Y} (Map ID: {player.Character.MapId}.");
                return;
            }

            await player.EmitEventAsync(new PortalTriggerEvent
            {
                Portal = currentPortal
            });
        }
    }
}