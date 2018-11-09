using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Entities.Player.Events
{
    public class PlayerLevelUpEvent : ChickenEventArgs
    {
        public IPlayerEntity Player { get; set; }

        public LevelUpType LevelUpType { get; set; }
    }
}

namespace ChickenAPI.Game.Entities.Player.Events
{
    public enum LevelUpType
    {
        Level,
        JobLevel,
        HeroLevel
    }
}