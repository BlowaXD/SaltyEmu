using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class ReputationGainEvent : GameEntityEvent
    {
        public long Reputation { get; set; }
        public long Dignity { get; set; }
    }
}