using ChickenAPI.Game.Entities.Player.Events;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class ExperienceManagerExtensions
    {
        public static void AddExperience(this IPlayerEntity player, long amount)
        {
            // todo implementation
            // send right packets
            // if levelUp
            player.EmitEvent(new PlayerLevelUpEvent
            {
                Player = player,
                LevelUpType = LevelUpType.Level,
            });
        }

        public static void AddJobExperience(this IPlayerEntity player, long amount)
        {
            // todo implementation
            // send right packets
            // if levelUp
            player.EmitEvent(new PlayerLevelUpEvent
            {
                Player = player,
                LevelUpType = LevelUpType.JobLevel,
            });
        }

        public static void AddHeroExperience(this IPlayerEntity player, long amount)
        {
            // todo implementation
            // send right packets
            // if levelUp
            player.EmitEvent(new PlayerLevelUpEvent
            {
                Player = player,
                LevelUpType = LevelUpType.HeroLevel,
            });
        }
    }
}