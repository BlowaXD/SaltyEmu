using System.Collections.Generic;
using System.Text;
using Autofac;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Enums;
using ChickenAPI.Utils;

namespace ChickenAPI.Data.Language
{
    public class EnChickenApiLanguageData : ChickenApiLanguageDataBase
    {
        public EnChickenApiLanguageData() : base(RegionType.English)
        {
            ChickenApiLanguageKeys = new Dictionary<LanguageKeys, string>
            {
                { LanguageKeys.SERVER_START, "[SERVER] Started" },
                { LanguageKeys.SERVER_STOP, "[SERVER] Stopped" },
                { LanguageKeys.LANGUAGE_INIT_FINISH, "[EN] Importing languages values" },
                { LanguageKeys.LANGUAGE_INIT_FINISH, "[EN] Imported {0} languages values" } // number of Languages values within the region
            };
        }
    }
}