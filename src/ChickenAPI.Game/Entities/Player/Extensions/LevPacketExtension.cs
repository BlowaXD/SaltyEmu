using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.Game.Server.Entities;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class LevPacketExtension
    {
        private static IAlgorithmService _algorithmService;
        private static IAlgorithmService Algorithm => _algorithmService ?? (_algorithmService = ChickenContainer.Instance.Resolve<IAlgorithmService>());

        public static LevPacket GenerateLevPacket(this IPlayerEntity player) =>
            new LevPacket
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

        public static LevelUpPacket GenerateLevelUpPacket(this IPlayerEntity player) =>
            new LevelUpPacket
            {
                CharacterId = player.Id
            };

        public static CharScPacket GenerateCharScPacket(this IPlayerEntity player)
        {
            return new CharScPacket
            {
                VisualType = player.Type,
                VisualId = player.Id,
                Size = player.Size,
            };
        }
    }
}