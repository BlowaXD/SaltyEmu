using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Effects.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.Effects
{
    public class AddEffectHandler : GenericEventPostProcessorBase<AddEffectEvent>
    {
        protected override Task Handle(AddEffectEvent e, CancellationToken cancellation)
        {
            var effects = e.Sender.GetComponent<EffectComponent>();

            if (effects == null)
            {
                effects = new EffectComponent(e.Sender);
                e.Sender.AddComponent(effects);
            }

            effects.Effects.Add(new EffectComponent.Effect(e.EffectId, e.Cooldown));
            return Task.CompletedTask;
        }
    }
}