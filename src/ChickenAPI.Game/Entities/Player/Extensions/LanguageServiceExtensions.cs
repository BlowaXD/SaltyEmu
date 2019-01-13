using System;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class LanguageServiceExtensions
    {
        private static readonly ILanguageService LanguageService = new Lazy<ILanguageService>(() => ChickenContainer.Instance.Resolve<ILanguageService>()).Value;

        public static string GetLanguageFormat(this IPlayerEntity player, ChickenI18NKey key, params object[] objs) => string.Format(LanguageService.GetLanguage(key, player.Session.Language), objs);

        public static string GetLanguage(this IPlayerEntity player, ChickenI18NKey key) => LanguageService.GetLanguage(key, player.Session.Language);
    }
}