using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game.Visibility.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Visibility
{
    public class Visibility_Invisible_Handler : GenericEventPostProcessorBase<VisibilitySetInvisibleEvent>
    {
        protected override async Task Handle(VisibilitySetInvisibleEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IVisibleCapacity visible))
            {
                return;
            }

            visible.Visibility = VisibilityType.Invisible;

            if (!e.Broadcast)
            {
                return;
            }

            if (e.Sender is IPlayerEntity player)
            {
                await player.BroadcastExceptSenderAsync(player.GenerateOutPacket());
            }
        }
    }
}