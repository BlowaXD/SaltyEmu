using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Player.Extension;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class ExperienceManagerExtensions
    {
        private static IAlgorithmService Algorithm => new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;

        public static void ChangeLevel(this IPlayerEntity player, byte level)
        {
            player.Character.Level = level;
            player.Character.LevelXp = 0;
            player.ActualizeUiExpBar();
        }

        public static void ChangeJobLevel(this IPlayerEntity player, byte level)
        {
            player.Character.JobLevel = level;
            player.Character.JobLevelXp = 0;
            player.ActualizeUiExpBar();
        }

        public static void ChangeReputation(this IPlayerEntity player, long reputation)
        {
            player.Character.Reput = reputation;
            player.ActualiseUiReputation();
        }

        public static void ChangeHeroLevel(this IPlayerEntity player, byte level)
        {
            player.Character.HeroLevel = level;
            player.Character.HeroXp = 0;
            player.ActualizeUiExpBar();
        }

        public static void TryLevelUp(this IPlayerEntity player)
        {
            int levelXp = Algorithm.GetLevelXp(player.Character.Class, player.Level);
            while (player.LevelXp >= levelXp)
            {
                player.LevelXp -= levelXp;
                player.Level++;
                levelXp = Algorithm.GetLevelXp(player.Character.Class, player.Level);
                player.EmitEvent(new PlayerLevelUpEvent
                {
                    Player = player,
                    LevelUpType = LevelUpType.Level,
                });
            }
        }

        public static void TryJobLevelUp(this IPlayerEntity player)
        {
            int jobXp = Algorithm.GetJobLevelXp(player.Character.Class, player.Level);
            while (player.JobLevelXp >= jobXp)
            {
                player.JobLevelXp -= jobXp;
                player.JobLevel++;
                jobXp = Algorithm.GetJobLevelXp(player.Character.Class, player.Level);
                player.EmitEvent(new PlayerLevelUpEvent
                {
                    Player = player,
                    LevelUpType = LevelUpType.JobLevel,
                });
            }
        }

        public static void TryHeroLevelUp(this IPlayerEntity player)
        {
            int heroXp = Algorithm.GetHeroLevelXp(player.Character.Class, player.Level);
            while (player.HeroLevelXp >= heroXp)
            {
                player.HeroLevelXp -= heroXp;
                player.HeroLevel++;
                heroXp = Algorithm.GetHeroLevelXp(player.Character.Class, player.Level);
                player.EmitEvent(new PlayerLevelUpEvent
                {
                    Player = player,
                    LevelUpType = LevelUpType.HeroLevel,
                });
            }
        }

        public static void AddExperience(this IPlayerEntity player, long amount)
        {
            player.LevelXp += amount;
            player.TryLevelUp();
            player.ActualizeUiExpBar();
        }

        public static void AddJobExperience(this IPlayerEntity player, long amount)
        {
            player.JobLevelXp += amount;
            player.TryJobLevelUp();
            player.ActualizeUiExpBar();
        }

        public static void AddHeroExperience(this IPlayerEntity player, long amount)
        {
            player.HeroLevelXp += amount;
            player.TryHeroLevelUp();
            player.ActualizeUiExpBar();
        }
    }
}