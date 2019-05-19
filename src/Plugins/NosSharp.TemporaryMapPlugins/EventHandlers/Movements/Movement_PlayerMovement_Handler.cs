using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Events;
using ChickenAPI.Game.Movements.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Movements
{
    public class Movement_PlayerMovement_Handler : GenericEventPostProcessorBase<PlayerMovementRequestEvent>
    {
        public Movement_PlayerMovement_Handler(ILogger log) : base(log)
        {
        }

        private static bool PreMovementChecks(IPlayerEntity player, PlayerMovementRequestEvent e)
        {
            // check for player' diseases
            if (player.Character.Hp == 0)
            {
                return false;
            }

            if (!player.CanMove(e.X, e.Y))
            {
                return false;
            }

            if (player.Position.X == e.X || player.Position.Y == e.Y)
            {
                return false;
            }

            if (player.Speed < e.Speed)
            {
                return false;
            }

            return true;
        }

        protected override async Task Handle(PlayerMovementRequestEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }
            if (!PreMovementChecks(player, e))
            {
                return;
            }

            player.Position.X = e.X;
            player.Position.Y = e.Y;

            await player.SendPacketAsync(player.GenerateCondPacket());
            await player.BroadcastAsync(player.GenerateMvPacket());
        }
    }
}