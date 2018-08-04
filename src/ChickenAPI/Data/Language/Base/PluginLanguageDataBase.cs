using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Server;

namespace ChickenAPI.Game.Data.Language.Base
{
    public abstract class PluginLanguageDataBase : IPluginLanguageData
    {
        protected static readonly Logger Log = Logger.GetLogger<ChickenApiLanguageDataBase>();

        protected readonly RegionType Region;
        protected Dictionary<string, string> LanguageData;

        protected PluginLanguageDataBase(RegionType region) => Region = region;

        public void Initialize()
        {
            var service = Container.Instance.Resolve<ILanguageService>();

            Log.Info(service.GetLanguage(LanguageKeys.LANGUAGE_INIT_START, Region));
            foreach (KeyValuePair<string, string> language in LanguageData)
            {
                service.SetLanguage(language.Key, language.Value, Region);
            }

            Log.Info(string.Format(service.GetLanguage(LanguageKeys.LANGUAGE_INIT_FINISH, Region), LanguageData.Count));
        }
    }
}