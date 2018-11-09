using System;
using System.Collections.Generic;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEventHandler : EventHandlerBase
    {
        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ExperienceGainEvent),
            typeof(ReputationGainEvent),
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case ExperienceGainEvent expGain:
                    if (!(entity is IPlayerEntity player))
                    {
                        return;
                    }

                    player.AddExperience(expGain.Experience);
                    player.AddJobExperience(expGain.JobExperience);
                    player.AddHeroExperience(expGain.HeroExperience);
                    break;
                case ReputationGainEvent repGain:
                    if (!(entity is IPlayerEntity session))
                    {
                        return;
                    }

                    break;
            }
        }
    }
}