using System;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game._i18n;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class LanguageServiceExtensions
    {
        private static readonly IGameLanguageService GameLanguageService = new Lazy<IGameLanguageService>(() => ChickenContainer.Instance.Resolve<IGameLanguageService>()).Value;

        public static string GetLanguageFormat(this IPlayerEntity player, PlayerMessages key, params object[] objs) => string.Format(GameLanguageService.GetLanguage(key, player.Session.Language), objs);

        public static string GetLanguage(this IPlayerEntity player, PlayerMessages key) => GameLanguageService.GetLanguage(key, player.Session.Language);
    }
}