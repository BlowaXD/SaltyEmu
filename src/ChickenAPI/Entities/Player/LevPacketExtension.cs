using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Features.Leveling;
using ChickenAPI.Game.Features.Skills.Extensions;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Entities.Player
{
    public static class LevPacketExtension
    {
        private static IAlgorithmService _algorithmService;
        private static IAlgorithmService Algorithm => _algorithmService ?? (_algorithmService = Container.Instance.Resolve<IAlgorithmService>());

        public static LevPacket GenerateLevPacket(this IPlayerEntity player)
        {
            var exp = player.GetComponent<ExperienceComponent>();
            return new LevPacket
            {
                Level = player.Experience.Level,
                LevelXp = player.Experience.LevelXp,
                JobLevel = exp.JobLevel,
                JobLevelXp = exp.JobLevelXp,
                LevelXpMax = Algorithm.GetLevelXp(player.Character.Class, exp.Level),
                JobLevelXpMax = Algorithm.GetLevelXp(player.Character.Class, exp.JobLevel),
                HeroLevel = exp.HeroLevel,
                Reputation = player.Character.Reput,
                Cp = player.GetCp(),
                HeroLevelXp = exp.HeroLevelXp,
                HeroLevelXpMax = Algorithm.GetHeroLevelXp(player.Character.Class, exp.HeroLevel),
            };
        }
    }
}