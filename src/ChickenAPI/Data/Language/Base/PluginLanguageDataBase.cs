using System.Collections.Generic;
using Autofac;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Enums;
using ChickenAPI.Utils;

namespace ChickenAPI.Data.Language.Base
{
    public abstract class PluginLanguageDataBase : IPluginLanguageData
    {
        protected PluginLanguageDataBase(RegionType region)
        {
            Region = region;
        }

        protected readonly RegionType Region;
        protected static readonly Logger Log = Logger.GetLogger<ChickenApiLanguageDataBase>();
        protected Dictionary<string, string> LanguageData;

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