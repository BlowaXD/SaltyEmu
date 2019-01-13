using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Battle.Events;
using ChickenAPI.Game.Battle.Hitting;
using ChickenAPI.Game.Battle.Interfaces;

namespace SaltyEmu.BasicPlugin.EventHandlers.Battle
{
    public class Battle_FillHitRequest_Handler : GenericEventPostProcessorBase<FillHitRequestEvent>
    {
        private readonly IDamageAlgorithm _algorithm;

        public Battle_FillHitRequest_Handler(IDamageAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        protected override Task Handle(FillHitRequestEvent e, CancellationToken cancellation)
        {
            HitRequest hitRequest = e.HitRequest;
            hitRequest.Damages = _algorithm.GenerateDamage(hitRequest);
            return Task.CompletedTask;
        }
    }
}