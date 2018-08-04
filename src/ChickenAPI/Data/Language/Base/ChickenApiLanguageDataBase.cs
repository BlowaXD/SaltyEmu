using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Server;

namespace ChickenAPI.Game.Data.Language.Base
{
    public abstract class ChickenApiLanguageDataBase : IChickenApiLanguageData
    {
        protected static readonly Logger Log = Logger.GetLogger<ChickenApiLanguageDataBase>();

        protected readonly RegionType Region;
        protected Dictionary<LanguageKeys, string> ChickenApiLanguageKeys;

        protected ChickenApiLanguageDataBase(RegionType region) => Region = region;

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