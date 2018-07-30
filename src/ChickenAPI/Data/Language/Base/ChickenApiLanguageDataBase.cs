using System.Collections.Generic;
using Autofac;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Enums;
using ChickenAPI.Utils;

namespace ChickenAPI.Data.Language
{
    public abstract class ChickenApiLanguageDataBase : IChickenApiLanguageData
    {
        protected ChickenApiLanguageDataBase(RegionType region)
        {
            Region = region;
        }

        protected readonly RegionType Region;
        protected static readonly Logger Log = Logger.GetLogger<ChickenApiLanguageDataBase>();
        protected Dictionary<LanguageKeys, string> ChickenApiLanguageKeys;

        public void Initialize()
        {
            var service = Container.Instance.Resolve<ILanguageService>();

            Log.Info(ChickenApiLanguageKeys[LanguageKeys.LANGUAGE_INIT_START]);
            foreach (KeyValuePair<LanguageKeys, string> language in ChickenApiLanguageKeys)
            {
                service.SetLanguage(language.Key, language.Value, Region);
            }

            Log.Info(string.Format(ChickenApiLanguageKeys[LanguageKeys.LANGUAGE_INIT_FINISH], ChickenApiLanguageKeys.Count));
        }
    }
}