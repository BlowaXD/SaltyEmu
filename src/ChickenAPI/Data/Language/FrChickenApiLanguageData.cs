using System.Collections.Generic;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.Language.Base;

namespace ChickenAPI.Game.Data.Language
{
    public class FrChickenApiLanguageData : ChickenApiLanguageDataBase
    {
        public FrChickenApiLanguageData() : base(RegionType.French) => ChickenApiLanguageKeys = new Dictionary<LanguageKeys, string>
        {
            { LanguageKeys.SERVER_START, "[SERVEUR] Démarré" },
            { LanguageKeys.SERVER_STOP, "[SERVEUR] Arrêt" },
            { LanguageKeys.LANGUAGE_INIT_FINISH, "[FR] Importation des clés de langue" },
            { LanguageKeys.LANGUAGE_INIT_FINISH, "[FR] {0} clés de langue importées" } // number of Languages values within the region
        };
    }
}