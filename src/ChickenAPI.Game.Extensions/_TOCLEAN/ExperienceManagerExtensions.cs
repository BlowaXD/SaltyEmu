using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player.Events;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class ExperienceManagerExtensions
    {
        private static IAlgorithmService Algorithm => new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;

        public static Task ChangeLevel(this IPlayerEntity player, byte level)
        {
            player.Character.Level = level;
            player.Character.LevelXp = 0;
            return player.ActualizeUiExpBar();
        }

        public static Task ChangeJobLevel(this IPlayerEntity player, byte level)
        {
            player.Character.JobLevel = level;
            player.Character.JobLevelXp = 0;
            return player.ActualizeUiExpBar();
        }

        public static Task ChangeReputation(this IPlayerEntity player, long reputation)
        {
            player.Character.Reput = reputation;
            return player.ActualizeUiReputation();
        }

        public static Task ChangeHeroLevel(this IPlayerEntity player, byte level)
        {
            player.Character.HeroLevel = level;
            player.Character.HeroXp = 0;
            return player.ActualizeUiExpBar();
        }

        public static async Task TryLevelUp(this IPlayerEntity player)
        {
            long levelXp = Algorithm.GetLevelXp(player.Character.Class, player.Level);
            while (player.LevelXp >= levelXp)
            {
                player.LevelXp -= levelXp;
                player.Level++;
                levelXp = Algorithm.GetLevelXp(player.Character.Class, player.Level);
                await player.EmitEventAsync(new PlayerLevelUpEvent
                {
                    Player = player,
                    LevelUpType = LevelUpType.Level
                });
            }
        }

        public static async Task TryJobLevelUp(this IPlayerEntity player)
        {
            int jobXp = Algorithm.GetJobLevelXp(player.Character.Class, player.Level);
            while (player.JobLevelXp >= jobXp)
            {
                player.JobLevelXp -= jobXp;
                player.JobLevel++;
                jobXp = Algorithm.GetJobLevelXp(player.Character.Class, player.Level);
                await player.EmitEventAsync(new PlayerLevelUpEvent
                {
                    Player = player,
                    LevelUpType = LevelUpType.JobLevel
                });
            }
        }

        public static async Task TryHeroLevelUp(this IPlayerEntity player)
        {
            int heroXp = Algorithm.GetHeroLevelXp(player.Character.Class, player.Level);
            while (player.HeroLevelXp >= heroXp)
            {
                player.HeroLevelXp -= heroXp;
                player.HeroLevel++;
                heroXp = Algorithm.GetHeroLevelXp(player.Character.Class, player.Level);
                await player.EmitEventAsync(new PlayerLevelUpEvent
                {
                    Player = player,
                    LevelUpType = LevelUpType.HeroLevel
                });
            }
        }

        public static async Task AddExperience(this IPlayerEntity player, long amount)
        {
            player.LevelXp += amount;
            await player.TryLevelUp();
            await player.ActualizeUiExpBar();
        }

        public static async Task AddJobExperience(this IPlayerEntity player, long amount)
        {
            player.JobLevelXp += amount;
            await player.TryJobLevelUp();
            await player.ActualizeUiExpBar();
        }

        public static async Task AddHeroExperience(this IPlayerEntity player, long amount)
        {
            player.HeroLevelXp += amount;
            await player.TryHeroLevelUp();
            await player.ActualizeUiExpBar();
        }
    }
}