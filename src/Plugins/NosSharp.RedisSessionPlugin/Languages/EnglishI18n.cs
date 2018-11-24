using System;
using System.Collections.Generic;
using ChickenAPI.Core.i18n;

namespace SaltyEmu.RedisWrappers.Languages
{
    public class EnglishI18n
    {
        public static Dictionary<ChickenI18NKey, string> Languages = new Dictionary<ChickenI18NKey, string>
        {
            { ChickenI18NKey.CHARACTER_NAME_ALREADY_TAKEN, "Character name is already taken" }
        };
    }
}