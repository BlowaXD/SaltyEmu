using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Locomotion.Events;
using ChickenAPI.Game.Movements.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Locomotion
{
    public class Locomotion_Untransform_Handler : GenericEventPostProcessorBase<LocomotionUntransformEvent>
    {
        private readonly IAlgorithmService _algorithm;

        public Locomotion_Untransform_Handler(IAlgorithmService algorithm)
        {
            _algorithm = algorithm;
        }

        protected override async Task Handle(LocomotionUntransformEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            player.Locomotion.IsVehicled = false;

            player.Speed = (byte)_algorithm.GetSpeed(player.Character.Class, player.Level);
            if (player.IsTransformedSp)
            {
                if (player.Sp != null)
                {
                    player.MorphId = player.Sp.Design;
                }
            }
            else
            {
                player.MorphId = 0;
            }

            await player.BroadcastAsync(player.GenerateCModePacket());
            await player.BroadcastAsync(player.GenerateCondPacket());
        }
    }
}