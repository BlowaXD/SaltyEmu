using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Locomotion.Events;
using ChickenAPI.Game.Movements.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Locomotion
{
    public class Locomotion_Transform_Handler : GenericEventPostProcessorBase<LocomotionTransformEvent>
    {
        protected override async Task Handle(LocomotionTransformEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            if (player.IsSitting)
            {
                player.IsSitting = false;
            }

            ItemDto item = e.Item.Item;

            player.Speed = item.Speed;
            player.LocomotionSpeed = item.Speed;
            player.Locomotion.IsVehicled = true;
            player.MorphId = (short)(item.Morph + (byte)player.Character.Gender);
            await player.BroadcastAsync(player.GenerateEffectPacket(196));
            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.BroadcastAsync(player.GenerateCondPacket());
        }
    }
}