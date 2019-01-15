using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class PlayerLevelUpEvent : GameEntityEvent
    {
        public IPlayerEntity Player { get; set; }

        public LevelUpType LevelUpType { get; set; }
    }
}