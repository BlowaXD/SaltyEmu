using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Enums.Game.Visibility;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game.Visibility.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Visibility
{
    public class Visibility_Visible_Handler : GenericEventPostProcessorBase<VisibilitySetVisibleEvent>
    {
        public Visibility_Visible_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(VisibilitySetVisibleEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IVisibleCapacity visible))
            {
                return;
            }

            visible.Visibility = VisibilityType.Visible;
            if (!e.Broadcast)
            {
                return;
            }

            if (e.Sender is IPlayerEntity player)
            {
                await player.BroadcastExceptSenderAsync(player.GenerateInPacket());
            }
            else
            {
                await e.Sender.CurrentMap.BroadcastAsync(e.Sender.GenerateInPacket());
            }
        }
    }
}