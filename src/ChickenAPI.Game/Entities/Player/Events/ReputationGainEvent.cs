using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class ReputationGainEvent : ChickenEventArgs
    {
        public long Reputation { get; set; }
        public long Dignity { get; set; }
    }
}