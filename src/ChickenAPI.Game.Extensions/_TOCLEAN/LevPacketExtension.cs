using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.ServerPackets.Entities;
using ChickenAPI.Packets.ServerPackets.Player;

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
                XpLoad = Algorithm.GetLevelXp(player.Character.Class, player.Level),
                JobXpLoad = Algorithm.GetLevelXp(player.Character.Class, player.JobLevel),
                HeroLevel = player.HeroLevel,
                Reputation = player.Character.Reput,
                SkillCp = player.GetCp(),
                HeroXp = (int)player.HeroLevelXp,
                HeroXpLoad = Algorithm.GetHeroLevelXp(player.Character.Class, player.HeroLevel)
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