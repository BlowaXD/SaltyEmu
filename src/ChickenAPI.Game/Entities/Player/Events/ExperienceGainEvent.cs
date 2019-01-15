using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class ExperienceGainEvent : GameEntityEvent
    {
        public long Experience { get; set; }
        public long JobExperience { get; set; }
        public long HeroExperience { get; set; }
    }
}