using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game.Movements.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Movements
{
    public class Movement_Sit_Handler : GenericEventPostProcessorBase<TriggerSitEvent>
    {
        protected override Task Handle(TriggerSitEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IMovableEntity movable))
            {
                return Task.CompletedTask;
            }

            movable.IsSitting = !movable.IsSitting;
            return Task.CompletedTask;
        }
    }
}