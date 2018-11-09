using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class ExperienceGainEvent : ChickenEventArgs
    {
        public long Experience { get; set; }
        public long JobExperience { get; set; }
        public long HeroExperience { get; set; }
    }
}