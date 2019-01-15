using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game._BroadcastRules;
using ChickenAPI.Packets.Game.Server.Map;

namespace SaltyEmu.BasicPlugin.EventHandlers.Maps
{
    public class Map_Leave_Handler : GenericEventPostProcessorBase<MapLeaveEvent>
    {
        protected override async Task Handle(MapLeaveEvent e, CancellationToken cancellation)
        {
            if (e.Sender is IPlayerEntity session)
            {
                await session.SendPacketAsync(new MapoutPacket());
                await e.Map.BroadcastAsync(e.Sender.GenerateOutPacket(), new AllExpectOne(session));
                return;
            }


            await e.Map.BroadcastAsync(e.Sender.GenerateOutPacket());
        }
    }
}