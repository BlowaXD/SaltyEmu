using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Features.Skills.Extensions;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class LevPacketExtension
    {
        private static IAlgorithmService _algorithmService;
        private static IAlgorithmService Algorithm => _algorithmService ?? (_algorithmService = ChickenContainer.Instance.Resolve<IAlgorithmService>());

        public static LevPacket GenerateLevPacket(this IPlayerEntity player)
        {
            return new LevPacket
            {
                Level = player.Level,
                LevelXp = (int)player.LevelXp,
                JobLevel = player.JobLevel,
                JobLevelXp = (int)player.JobLevelXp,
                LevelXpMax = Algorithm.GetLevelXp(player.Character.Class, player.Level),
                JobLevelXpMax = Algorithm.GetLevelXp(player.Character.Class, player.JobLevel),
                HeroLevel = player.HeroLevel,
                Reputation = player.Character.Reput,
                Cp = player.GetCp(),
                HeroLevelXp = (int)player.HeroLevelXp,
                HeroLevelXpMax = Algorithm.GetHeroLevelXp(player.Character.Class, player.HeroLevel)
            };
        }
    }
}