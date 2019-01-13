using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Entities.Player.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Player_ExperienceGain_Handler : GenericEventPostProcessorBase<ExperienceGainEvent>
    {
        protected override async Task Handle(ExperienceGainEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            player.AddExperience(e.Experience);
            player.AddJobExperience(e.JobExperience);
            player.AddHeroExperience(e.HeroExperience);
        }
    }
}